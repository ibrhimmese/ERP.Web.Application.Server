using ERPServer.Domain.Entities;
using ERPServer.Domain.Repository;
using ERPServer.Infrastructure.Context;
using GenericRepository;

namespace ERPServer.Infrastructure.Repositories;

internal sealed class InvoiceDetailRepository : Repository<InvoiceDetail, ApplicationDbContext>, IInvoiceDetailRepository
{
    public InvoiceDetailRepository(ApplicationDbContext context) : base(context)
    {
    }
}
