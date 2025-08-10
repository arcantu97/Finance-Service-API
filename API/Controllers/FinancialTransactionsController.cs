using FinanceService.Application.Transactions;
using FinanceService.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialTransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;
        private readonly ILogger<FinancialTransactionsController> _logger;

        public FinancialTransactionsController(ITransactionsService transactionsService, ILogger<FinancialTransactionsController> logger)
        {
            _transactionsService = transactionsService;
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
                var transactions = await _transactionsService.GetAllAsync();
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
                var transaction = await _transactionsService.GetByIdAsync(id);
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _transactionsService.CreateAsync(transaction);
                _logger.LogInformation($"Transacción creada correctamente");
                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear transacción.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Regla de negocio al crear transacción.");
                return UnprocessableEntity(ex.Message);
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

            try
            {
                await _transactionsService.UpdateAsync(id, transaction);
                _logger.LogInformation($"Transacción actualizada correctamente.");
                return NoContent();
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
                await _transactionsService.DeleteAsync(id);
                _logger.LogInformation($"Transacción eliminada correctamente.");

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
                await _transactionsService.DeleteAllAsync();
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
