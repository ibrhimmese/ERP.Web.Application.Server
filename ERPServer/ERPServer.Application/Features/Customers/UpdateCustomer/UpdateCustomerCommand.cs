using ERPServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Customers.UpdateCustomer;

public sealed record UpdateCustomerCommand(
    Guid id,
    string Name,
    string TaxDepartment,
    string TaxNumber,
    string City,
    string Town,
    string FullAddress
    ) :IRequest<Result<string>>;
