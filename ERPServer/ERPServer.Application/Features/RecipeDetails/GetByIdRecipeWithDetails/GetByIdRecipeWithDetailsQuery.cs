using ERPServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.RecipeDetails.GetByIdRecipeWithDetails;

public sealed record GetByIdRecipeWithDetailsQuery(Guid RecipeId) : IRequest<Result<Recipe>>;
