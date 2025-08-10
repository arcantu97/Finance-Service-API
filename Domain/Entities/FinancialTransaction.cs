using FinanceService.Enums;

namespace FinanceService.Models
{
    public class FinancialTransaction
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public CategoryType Category { get; set; } 
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public TransactionType? Type { get; set; }
    }
}