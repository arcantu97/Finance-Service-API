using FinanceService.Enums;

namespace FinanceService.Models
{
   public class Saving
    {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // "Ahorro para vacaciones"
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public SavingType Type { get; set; } = SavingType.Manual;

    public RecurringTransactionType? Recurrence { get; set; }
    public bool IsGoalLinked { get; set; } = false;
    public int? GoalId { get; set; }
}

}