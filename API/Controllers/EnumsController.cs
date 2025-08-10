using FinanceService.Enums;
using Microsoft.AspNetCore.Mvc;


namespace FinanceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnumsController : ControllerBase
    {
        /// <summary>
        /// Devuelve los tipos de transacción disponibles (enum TransactionType).
        /// </summary>
        [HttpGet("transaction-types")]
        public IActionResult GetTransactionTypes()
        {
            var values = Enum.GetValues(typeof(TransactionType))
                .Cast<TransactionType>()
                .Select(e => new { Id = (int)e, Name = e.ToString() });
            return Ok(values);
        }

        /// <summary>
        /// Devuelve las categorías de gasto disponibles (enum ExpenseCategory).
        /// </summary>
        [HttpGet("expense-categories")]
        public IActionResult GetExpenseCategories()
        {
            var values = Enum.GetValues(typeof(CategoryType))
                .Cast<CategoryType>()
                .Select(e => new { Id = (int)e, Name = e.ToString() });
            return Ok(values);
        }

        /// <summary>
        /// Devuelve los tipos de ahorro disponibles (enum SavingType).
        /// </summary>
        [HttpGet("saving-types")]
        public IActionResult GetSavingTypes()
        {
            var values = Enum.GetValues(typeof(SavingType))
                .Cast<SavingType>()
                .Select(e => new { Id = (int)e, Name = e.ToString() });
            return Ok(values);
        }

        /// <summary>
        /// Devuelve los tipos de transacción recurrente disponibles (enum RecurringTransactionType).
        /// </summary>
        [HttpGet("recurring-transaction-types")]
        public IActionResult GetRecurringTransactionTypes()
        {
            var values = Enum.GetValues(typeof(RecurringTransactionType))
                .Cast<RecurringTransactionType>()
                .Select(e => new { Id = (int)e, Name = e.ToString() });
            return Ok(values);
        }

        /// <summary>
        /// Devuelve los estados de las metas disponibles (enum GoalStatusType).
        /// </summary>
        [HttpGet("goal-status-types")]
        public IActionResult GetGoalStatusTypes()
        {
            var values = Enum.GetValues(typeof(GoalStatusType))
                .Cast<GoalStatusType>()
                .Select(e => new { Id = (int)e, Name = e.ToString() });
            return Ok(values);
        }
        
    }
}
