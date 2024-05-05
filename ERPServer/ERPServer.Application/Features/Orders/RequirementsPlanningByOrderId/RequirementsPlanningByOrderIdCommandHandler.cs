using ERPServer.Domain.Dtos;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Enums;
using ERPServer.Domain.Repository;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Orders.RequirementsPlanningByOrderId;

internal sealed class RequirementsPlanningByOrderIdCommandHandler(
    IOrderRepository orderRepository,
    IStockMovementRepository  stockMovementRepository ,
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<RequirementsPlanningByOrderIdCommand, Result<RequirementsPlanningByOrderIdCommandResponse>>
{
    public async Task<Result<RequirementsPlanningByOrderIdCommandResponse>> Handle(RequirementsPlanningByOrderIdCommand request, CancellationToken cancellationToken)
    {
        Order? order = await orderRepository
            .Where(p=>
            p.Id==request.OrderId)
            .Include(p=>p.Details!)
            .ThenInclude(p=>p.Product)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return Result<RequirementsPlanningByOrderIdCommandResponse>.Failure("Sipariş Bulunamadı");
        }
        List<ProductDto> uretilmesiGerekenUrunlistesi = new();
        List<ProductDto> requirementsPlanningProducts = new();
        if(order.Details is not null)
        {
            foreach (var item in order.Details)
            {
                var product = item.Product;
                List<StockMovement> movements = await stockMovementRepository
                    .Where(p => p.ProductId == product!.Id)
                    .ToListAsync(cancellationToken);

                decimal stock = movements.Sum(p => p.NumberOfEntries) - movements.Sum(p => p.NumberOfOutputs);
                if (stock < item.Quantity) 
                {
                    ProductDto uretilmesiGerekenUrun = new()
                    {
                        Id = item.ProductId,
                        Name = product!.Name,
                        Quantity = item.Quantity - stock
                    };
                    uretilmesiGerekenUrunlistesi.Add(uretilmesiGerekenUrun);
                }
            }

            foreach (var item in uretilmesiGerekenUrunlistesi)
            {
                Recipe? recipe = await recipeRepository
                    .Where(p => p.ProductId == item.Id)
                    .Include(p => p.Details!)
                    .ThenInclude(p=>p.Product)
                    .FirstOrDefaultAsync(cancellationToken);

                if(recipe is not null && recipe.Details is not null)
                {
                    foreach (var detail in recipe.Details)
                    {
                        List<StockMovement> urunMovements = await stockMovementRepository
                        .Where(p => p.ProductId == detail.ProductId)
                        .ToListAsync(cancellationToken);

                        decimal stock = urunMovements.Sum(p => p.NumberOfEntries) - urunMovements.Sum(p => p.NumberOfOutputs);
                        if (stock < detail.Quantity)
                        {
                            ProductDto ihtiyacOlanUrun = new()
                            {
                                Id = detail.ProductId,
                                Name = detail.Product!.Name,
                                Quantity = detail.Quantity - stock
                            };
                            requirementsPlanningProducts.Add(ihtiyacOlanUrun);
                        }
                    }


                }
            }
        }


        requirementsPlanningProducts = requirementsPlanningProducts.GroupBy(p => p.Id).Select(g => new ProductDto
        {
            Id = g.Key,
            Name = g.First().Name,
            Quantity = g.Sum(item => item.Quantity)
        }).ToList();

        order.Status = OrderStatusEnum.RequirentmentsPlanWorked;
        orderRepository.Update(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RequirementsPlanningByOrderIdCommandResponse(
            DateOnly.FromDateTime(DateTime.Now),
            order.Number + " Nolu Siparişin İhtiyaç Planlaması", requirementsPlanningProducts);

    }
}



//Siparişteki ürünlerin tüm depolardaki adetlerine bakılacak.
//Eğer yeterli sayıda yok ise kaç adet üretilecek?
//Ürünler için gereken ürün reçetesi alınıp stok adetlerime bakılacak.
//Üretilmesi için gereken ürünlerden kaçına ihtiyaç olduğunu tespit edip liste olarak döndürülecek.