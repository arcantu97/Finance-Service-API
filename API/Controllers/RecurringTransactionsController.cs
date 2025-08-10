using FinanceService.Data;
using FinanceService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecurringTransactionsController : ControllerBase
    {
        private readonly FinanceContext _context;

        public RecurringTransactionsController(FinanceContext context)
        {
            _context = context;
        }

        /// <summary>Obtiene todas las transacciones recurrentes.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecurringTransaction>>> GetRecurringTransactions()
        {
            return await _context.RecurringTransactions.ToListAsync();
        }

        /// <summary>Obtiene una transacción recurrente por su ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RecurringTransaction>> GetRecurringTransaction(int id)
        {
            var recurringTransaction = await _context.RecurringTransactions.FindAsync(id);
            if (recurringTransaction == null)
                return NotFound();
            return recurringTransaction;
        }

        /// <summary>Crea una nueva transacción recurrente.</summary>
        [HttpPost]
        public async Task<ActionResult<RecurringTransaction>> PostRecurringTransaction(RecurringTransaction recurringTransaction)
        {
            _context.RecurringTransactions.Add(recurringTransaction);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRecurringTransaction), new { id = recurringTransaction.Id }, recurringTransaction);
        }

        /// <summary>Actualiza una transacción recurrente existente.</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecurringTransaction(int id, RecurringTransaction recurringTransaction)
        {
            if (id != recurringTransaction.Id)
                return BadRequest();

            _context.Entry(recurringTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RecurringTransactions.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Elimina una transacción recurrente por ID.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecurringTransaction(int id)
        {
            var recurringTransaction = await _context.RecurringTransactions.FindAsync(id);
            if (recurringTransaction == null)
                return NotFound();

            _context.RecurringTransactions.Remove(recurringTransaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}