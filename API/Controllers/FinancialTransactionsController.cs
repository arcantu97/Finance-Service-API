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
        private readonly ILogger<FinancialTransactionsController> _logger;

        public FinancialTransactionsController(FinanceContext context, ILogger<FinancialTransactionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los movimientos financieros.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialTransaction>>> GetTransactions()
        {
            try
            {
                var transactions = await _context.Transactions.ToListAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las transacciones.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        /// Obtiene una transacción financiera por su ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialTransaction>> GetTransaction(int id)
        {
            try
            {
                var transaction = await _context.Transactions.FindAsync(id);
                if (transaction == null)
                    return NotFound();

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la transacción con ID {id}.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        /// Crea una nueva transacción financiera.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FinancialTransaction>> PostTransaction(FinancialTransaction transaction)
        {
            try
            {
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una transacción.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        /// Actualiza una transacción financiera existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, FinancialTransaction transaction)
        {
            if (id != transaction.Id)
                return BadRequest("El ID en la URL no coincide con el cuerpo de la solicitud.");

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Transactions.Any(e => e.Id == id))
                    return NotFound();

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar la transacción con ID {id}.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        /// Elimina una transacción financiera por ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                var transaction = await _context.Transactions.FindAsync(id);
                if (transaction == null)
                    return NotFound();

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar la transacción con ID {id}.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        /// Elimina todas las transacciones financieras.
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteAllTransactions()
        {
            try
            {
                var transactions = await _context.Transactions.ToListAsync();
                if (!transactions.Any())
                    return NotFound("No hay transacciones para eliminar.");

                _context.Transactions.RemoveRange(transactions);
                await _context.SaveChangesAsync();

                await _context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='Transactions'");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar todas las transacciones.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
