using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ReceivablesContext : DbContext
    {
        public DbSet<Debtor> Debtors { get; set; }
        public DbSet<Receivable> Receivables { get; set; }

        public ReceivablesContext(DbContextOptions options) : base(options)
        {
        }
    }
}
