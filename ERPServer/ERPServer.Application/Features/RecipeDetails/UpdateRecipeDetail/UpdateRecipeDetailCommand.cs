using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repository;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.RecipeDetails.UpdateRecipeDetail;

public sealed record UpdateRecipeDetailCommand(
    Guid Id,
    Guid ProductId,
    decimal Quantity
    ):IRequest<Result<string>>;


internal sealed class UpdateRecipeDetailCommandHandler(
    IRecipeDetailRepository recipeDetailRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<UpdateRecipeDetailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateRecipeDetailCommand request, CancellationToken cancellationToken)
    {
        RecipeDetail recipeDetail = await recipeDetailRepository
            .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);
        if (recipeDetail == null)
        {
            return Result<string>.Failure("Bu ürüne ait bir reçete bulunamadı");
        }

        RecipeDetail? oldRecipeDetail = await recipeDetailRepository
            .Where(p=>
            p.Id!= request.Id &&
            p.ProductId==request.ProductId &&
            p.RecipeId==recipeDetail.RecipeId
            ).FirstOrDefaultAsync(cancellationToken);
        if (oldRecipeDetail is not null)
        {
            recipeDetailRepository.Delete(recipeDetail);
            oldRecipeDetail.Quantity += request.Quantity;
            recipeDetailRepository.Update(oldRecipeDetail);

        }
        else
        {
            mapper.Map(request, recipeDetail);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Reçetedeki ürün başarıyla güncellendi";
    }
}
