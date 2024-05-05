using ERPServer.Domain.Entities;
using ERPServer.Domain.Repository;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Invoices.DeleteInvoiceById;

internal sealed class DeleteInvoiceByIdCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork,
    IStockMovementRepository stockMovementRepository

    ) : IRequestHandler<DeleteInvoiceByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteInvoiceByIdCommand request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await invoiceRepository.GetByExpressionAsync(p=>p.Id==request.Id,cancellationToken);

        if (invoice == null)
        {
            return Result<string>.Failure("Fatura Bulunamadı");
        }
        List<StockMovement> movements = await stockMovementRepository
            .Where(p => p.InvoiceId == invoice.Id)
            .ToListAsync(cancellationToken);

        stockMovementRepository.DeleteRange(movements);
             
        invoiceRepository.Delete(invoice);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Fatura Başarıyla Silindi";

    }
}
