using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ReceivableRepository : IReceivableRepository
    {
        private readonly ReceivablesContext _context;

        public ReceivableRepository(ReceivablesContext context)
        {
            _context = context;
        }

        public async Task<Debtor?> GetDebtorByReference(int reference)
        {
            return await _context.Debtors
                .Include(d => d.Receivables)
                .FirstOrDefaultAsync(d => d.Reference == reference);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddDebtor(Debtor debtor)
        {
            await _context.Debtors.AddAsync(debtor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReceivable(Receivable receivable)
        {
            var existingReceivable = await _context.Receivables
                .Where(rec => rec.Reference == receivable.Reference)
                .FirstOrDefaultAsync();

            if (existingReceivable != null)
            {
                existingReceivable.CurrencyCode = receivable.CurrencyCode;
                existingReceivable.IssueDate = receivable.IssueDate;
                existingReceivable.OpeningValue = receivable.OpeningValue;
                existingReceivable.PaidValue = receivable.PaidValue;
                existingReceivable.DueDate = receivable.DueDate;
                existingReceivable.ClosedDate = receivable.ClosedDate;
                existingReceivable.Cancelled = receivable.Cancelled;
            }
        }

        public async Task<IEnumerable<Receivable>> GetClosedInvoices()
        {
            return await _context.Receivables.Where(rec => rec.Cancelled == false && rec.ClosedDate != null).ToListAsync();
        }

        public async Task<IEnumerable<Receivable>> GetOpenInvoices()
        {
            return await _context.Receivables.Where(rec => rec.Cancelled == false && rec.ClosedDate == null).ToListAsync();
        }
    }
}
