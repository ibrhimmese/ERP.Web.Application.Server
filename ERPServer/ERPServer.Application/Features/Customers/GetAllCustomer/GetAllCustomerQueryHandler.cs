using ERPServer.Domain.Entities;
using ERPServer.Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Customers.GetAllCustomer;

internal sealed class GetAllCustomerQueryHandler(
    ICustomerRepository customerRepository
    ) : IRequestHandler<GetAllCustomerQuery, Result<List<ERPServer.Domain.Entities.Customer>>>
{
    public async Task<Result<List<ERPServer.Domain.Entities.Customer>>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
    {
        List<ERPServer.Domain.Entities.Customer> customers= await  customerRepository.GetAll().OrderBy(p=>p.Name).ToListAsync(cancellationToken);
        return customers;
    }
}
