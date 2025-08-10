using FinanceService.Data;
using FinanceService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialTransactionsController : ControllerBase
    {
        private readonly FinanceContext _context;
        public FinancialTransactionsController(FinanceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los movimientos financieros.
        /// </summary>
        /// <returns>Lista de transacciones financieras.</returns>
        // GET: api/transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialTransaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        /// <summary>
        /// Obtiene una transacción financiera por su ID.
        /// </summary>
        /// <param name="id">ID de la transacción.</param>
        /// <returns>La transacción financiera si se encuentra; otherwise NotFound.</returns>
        // GET: api/transactions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialTransaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return transaction;
        }

        /// <summary>
        /// Crea una nueva transacción financiera.
        /// </summary>
        /// <param name="transaction">Objeto FinancialTransaction a crear.</param>
        /// <returns>La transacción creada con su URI.</returns>
        // POST: api/transactions
        [HttpPost]
        public async Task<ActionResult<FinancialTransaction>> PostTransaction(FinancialTransaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }

        /// <summary>
        /// Actualiza una transacción financiera existente.
        /// </summary>
        /// <param name="id">ID de la transacción a actualizar.</param>
        /// <param name="transaction">Objeto FinancialTransaction con datos actualizados.</param>
        /// <returns>NoContent si se actualizó, BadRequest o NotFound si hubo problemas.</returns>
        // PUT: api/transactions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, FinancialTransaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }
            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Transactions.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Elimina una transacción financiera por ID.
        /// </summary>
        /// <param name="id">ID de la transacción a eliminar.</param>
        /// <returns>NoContent si se eliminó, NotFound si no existe.</returns>
        // DELETE: api/transactions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Elimina todas las transacciones financieras.
        /// </summary>
        /// <returns>NoContent si se eliminaron todas las transacciones.</returns>
        // DELETE: api/transactions
        [HttpDelete()]
        public async Task<IActionResult> DeleteTransactios()
        {

            var transactions = await _context.Transactions.ToListAsync();
            if (transactions.Count == 0)
            {
                return NotFound();
            }
            _context.Transactions.RemoveRange(transactions);
            await _context.SaveChangesAsync();

            // Reinicia el ID de la tabla
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='Transactions'");
            return NoContent();
        }
    }
}