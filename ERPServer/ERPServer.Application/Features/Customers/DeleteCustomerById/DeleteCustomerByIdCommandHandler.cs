using ERPServer.Domain.Repository;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Customers.DeleteCustomerById;

internal sealed class DeleteCustomerByIdCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteCustomerByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteCustomerByIdCommand request, CancellationToken cancellationToken)
    {
        ERPServer.Domain.Entities.Customer customer = await customerRepository.GetByExpressionAsync(p => p.Id == request.id, cancellationToken);
        if (customer == null)
        {
            return Result<string>.Failure("Müşteri bulunamadı");
        }
        customerRepository.Delete(customer);
        await unitOfWork.SaveChangesAsync();

        return "Müşteri Başarıyla Silindi";
    }
}

