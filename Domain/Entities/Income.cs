using FinanceService.Models.Enums;

namespace FinanceService.Models
{
    public class Income
    {
        public int Id { get; set; }
        public string Source { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public RecurringTransactionType Recurrence { get; set; } = RecurringTransactionType.None;   
        
    }
}