using FluentValidation;

namespace ERPServer.Application.Features.Customers.CreateCustomer;

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(p=>p.TaxNumber).MinimumLength(10).MaximumLength(11).WithMessage("Vergi Numarası TC veya Vergi Numaranız olmalıdır");
        RuleFor(p => p.Name).MinimumLength(3).WithMessage("İsim alanı minimum 3 harf olmalıdır");
    }
}