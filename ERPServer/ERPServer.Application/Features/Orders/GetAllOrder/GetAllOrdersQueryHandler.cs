using ERPServer.Domain.Entities;
using ERPServer.Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Orders.GetAllOrder;

internal sealed class GetAllOrdersQueryHandler(
    IOrderRepository orderRepository
    ) : IRequestHandler<GetAllOrdersQuery, Result<List<Order>>>
{
    public async Task<Result<List<Order>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        List<Order> orders = await orderRepository
            .GetAll()
            .Include(p => p.Customer)
            .Include(p => p.Details!)
            .ThenInclude(p => p.Product)
            .OrderByDescending(p => p.Date)
            .ToListAsync(cancellationToken);

        return orders;
    }
}

