using ERPServer.Domain.Dtos;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Orders.RequirementsPlanningByOrderId;

public sealed record RequirementsPlanningByOrderIdCommand(
    Guid OrderId
    ):IRequest<Result<RequirementsPlanningByOrderIdCommandResponse>>;

public sealed record RequirementsPlanningByOrderIdCommandResponse(
    DateOnly Date,
    string Title,
    List<ProductDto> Products
    );
