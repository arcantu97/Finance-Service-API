using FinanceService.Data;
using FinanceService.Domain.Repositories;
using FinanceService.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FinanceContext _context;
        public TransactionRepository(FinanceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddTransactionAsync(FinancialTransaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllTransactionsAsync()
        {
            var range = _context.Transactions;
            _context.Transactions.RemoveRange(range);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='Transactions'");
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions.FirstAsync(t => t.Id == id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Transaction with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<FinancialTransaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<FinancialTransaction> GetTransactionByIdAsync(int id)
        {
               var transaction = await _context.Transactions.FirstAsync(t => t.Id == id);
            if (transaction != null)
            {
                return transaction;
            }
            else
            {
                throw new KeyNotFoundException($"Transaction with ID {id} not found.");
            }
        }

        public async Task UpdateTransactionAsync(FinancialTransaction transaction)
        {
            _context.Entry(transaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}