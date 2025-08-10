using FinanceService.Models;

namespace FinanceService.Application.Transactions
{
    public interface ITransactionsService
    {
        Task<IEnumerable<FinancialTransaction>> GetAllAsync();
        Task<FinancialTransaction?> GetByIdAsync(int id);
        Task<FinancialTransaction> CreateAsync(FinancialTransaction transaction);
        Task UpdateAsync(int id, FinancialTransaction transaction);
        Task DeleteAsync(int id);
        Task DeleteAllAsync();
    }
}
