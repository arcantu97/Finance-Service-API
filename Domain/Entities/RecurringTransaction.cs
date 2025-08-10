using FinanceService.Enums;

namespace FinanceService.Models
{
    public class RecurringTransaction
    {
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public RecurringTransactionType Recurrence { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public TransactionType Type { get; set; }
    public bool IsActive { get; set; } = true;
    }
}