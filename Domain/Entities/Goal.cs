using FinanceService.Enums;
namespace FinanceService.Models
{
    public class Goal
    {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; } = 0;
    public DateTime TargetDate { get; set; }
    public GoalStatusType Status { get; set; } = GoalStatusType.Active;
    }
}