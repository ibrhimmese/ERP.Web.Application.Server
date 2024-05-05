using ERPServer.Domain.Entities;
using ERPServer.Domain.Repository;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Recipes.CreateRecipe;

internal sealed class CreateRecipeCommandHandler(
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork
 
    ) : IRequestHandler<CreateRecipeCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        bool isRecipeExist = await recipeRepository.AnyAsync(p => p.ProductId == request.ProductId, cancellationToken);
        if (isRecipeExist)
        {
            return Result<string>.Failure("Bu Ürüne ait bir reçete bulunmaktadır");
        }

        Recipe recipe = new()
        {
            ProductId = request.ProductId,
            Details = request.Details
            .Select(s =>
                new RecipeDetail()
                {
                    ProductId = s.ProductId,
                    Quantity = s.Quantity

                }).ToList()
        };
        await recipeRepository.AddAsync(recipe, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Reçete kaydı Başarıyla Oluşturuldu";
    }
}
