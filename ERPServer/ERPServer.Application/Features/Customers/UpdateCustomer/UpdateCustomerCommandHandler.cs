using AutoMapper;
using ERPServer.Domain.Repository;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Customers.UpdateCustomer;

internal sealed class UpdateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<UpdateCustomerCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        ERPServer.Domain.Entities.Customer customer = await customerRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.id, cancellationToken);
        if (customer == null)
        {
            return Result<string>.Failure("Müşteri bulunamadı");
        }

        if(customer.TaxNumber != request.TaxNumber)
        {
            bool isTaxNumberExist = await customerRepository.AnyAsync(p => p.TaxNumber == request.TaxNumber, cancellationToken);

            if (isTaxNumberExist)
            {
                return Result<string>.Failure("Vergi Numarası Daha önce kullanılmıştır");
            }
        }
        mapper.Map(request, customer);

       //** customerRepository.Update(customer); bu satıra gerek yok getByExpressionWtihTrackingAsync kullandım //**
       
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Müşteri bilgileri başarıyla güncellendi";
    }
}
