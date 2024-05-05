using ERPServer.Domain.Entities;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Orders.GetAllOrder;

public sealed record class GetAllOrdersQuery():IRequest<Result<List<Order>>>;

