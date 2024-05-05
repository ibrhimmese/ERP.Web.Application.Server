using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repository;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Orders.CreateOrder;

internal sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<CreateOrderCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Order? lastOrder = await orderRepository
            .Where(p => 
            p.OrderNumberYear == request.Date.Year)
            .OrderByDescending(p => p.OrderNumber)
            .FirstOrDefaultAsync(cancellationToken);

        int lastOrderNumber = 0;
        if (lastOrder != null)
        {
            lastOrderNumber= lastOrder.OrderNumber;
        }

        List<OrderDetail> details = request.Details.Select(s => new OrderDetail
        {
            Price = s.Price,
            ProductId = s.ProductId,
            Quantity = s.Quantity,
        }).ToList();

        Order order = mapper.Map<Order>(request);
        order.OrderNumber= lastOrderNumber+1;
        order.OrderNumberYear= request.Date.Year;


        await orderRepository.AddAsync( order,cancellationToken );
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Sipariş Başarıyla Oluşturuldu";
    }
}
