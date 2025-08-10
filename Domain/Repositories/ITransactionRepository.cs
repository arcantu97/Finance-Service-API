using FinanceService.Models;

namespace FinanceService.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<FinancialTransaction>> GetAllTransactionsAsync();
        Task<FinancialTransaction> GetTransactionByIdAsync(int id);
        Task AddTransactionAsync(FinancialTransaction transaction);
        Task UpdateTransactionAsync(FinancialTransaction transaction);
        Task DeleteTransactionAsync(int id);
        Task DeleteAllTransactionsAsync();
    }
}