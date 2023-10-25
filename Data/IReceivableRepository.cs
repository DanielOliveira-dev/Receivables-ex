using Domain;

namespace Data
{
    public interface IReceivableRepository
    {
        Task<Debtor?> GetDebtorByReference(int reference);
        Task SaveChangesAsync();
        Task<IEnumerable<Receivable>> GetClosedInvoices();
        Task<IEnumerable<Receivable>> GetOpenInvoices();
        Task AddDebtor(Debtor debtor);
        Task UpdateReceivable(Receivable receivable);
    }
}
