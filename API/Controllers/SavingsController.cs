using FinanceService.Data;
using FinanceService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavingsController : ControllerBase
    {
        private readonly FinanceContext _context;

        public SavingsController(FinanceContext context)
        {
            _context = context;
        }

        /// <summary>Obtiene todos los ahorros.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Saving>>> GetSavings()
        {
            return await _context.Savings.ToListAsync();
        }

        /// <summary>Obtiene un ahorro por su ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Saving>> GetSaving(int id)
        {
            var saving = await _context.Savings.FindAsync(id);
            if (saving == null)
                return NotFound();
            return saving;
        }

        /// <summary>Crea un nuevo ahorro.</summary>
        [HttpPost]
        public async Task<ActionResult<Saving>> PostSaving(Saving saving)
        {
            _context.Savings.Add(saving);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSaving), new { id = saving.Id }, saving);
        }

        /// <summary>Actualiza un ahorro existente.</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaving(int id, Saving saving)
        {
            if (id != saving.Id)
                return BadRequest();

            _context.Entry(saving).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Savings.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Elimina un ahorro por ID.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaving(int id)
        {
            var saving = await _context.Savings.FindAsync(id);
            if (saving == null)
                return NotFound();

            _context.Savings.Remove(saving);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
