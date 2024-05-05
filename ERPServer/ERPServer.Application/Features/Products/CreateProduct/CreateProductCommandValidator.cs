using ERPServer.Domain.Entities;
using FluentValidation;

namespace ERPServer.Application.Features.Products.CreateProduct;

public sealed class CreateProductCommandValidator:AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.ProductTypeValue).GreaterThan(0);
    }
}
