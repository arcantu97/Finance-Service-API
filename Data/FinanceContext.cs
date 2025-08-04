using FinanceService.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Data
{
    public class FinanceContext(DbContextOptions<FinanceContext> options) : DbContext(options)
    {
        public DbSet<FinancialTransaction> Transactions { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
        public DbSet<Saving> Savings { get; set; }

    }
}