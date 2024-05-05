using ERPServer.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Customers.DeleteCustomerById;

public sealed record DeleteCustomerByIdCommand(Guid id):IRequest<Result<string>>;

