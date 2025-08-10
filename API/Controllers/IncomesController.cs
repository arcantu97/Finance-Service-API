using FinanceService.Data;
using FinanceService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomesController : ControllerBase
    {
        private readonly FinanceContext _context;

        public IncomesController(FinanceContext context)
        {
            _context = context;
        }

        /// <summary>Obtiene todos los ingresos.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Income>>> GetIncomes()
        {
            return await _context.Incomes.ToListAsync();
        }

        /// <summary>Obtiene un ingreso por su ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Income>> GetIncome(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
                return NotFound();
            return income;
        }

        /// <summary>Crea un nuevo ingreso.</summary>
        [HttpPost]
        public async Task<ActionResult<Income>> PostIncome(Income income)
        {
            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, income);
        }

        /// <summary>Actualiza un ingreso existente.</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncome(int id, Income income)
        {
            if (id != income.Id)
                return BadRequest();

            _context.Entry(income).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Incomes.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Elimina un ingreso por ID.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
                return NotFound();

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
