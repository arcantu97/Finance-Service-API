using FinanceService.Data;
using FinanceService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly FinanceContext _context;

        public GoalsController(FinanceContext context)
        {
            _context = context;
        }

        /// <summary>Obtiene todas las metas.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Goal>>> GetGoals()
        {
            return await _context.Goals.ToListAsync();
        }

        /// <summary>Obtiene una meta por su ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Goal>> GetGoal(int id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
                return NotFound();
            return goal;
        }

        /// <summary>Crea una nueva meta.</summary>
        [HttpPost]
        public async Task<ActionResult<Goal>> PostGoal(Goal goal)
        {
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGoal), new { id = goal.Id }, goal);
        }

        /// <summary>Actualiza una meta existente.</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGoal(int id, Goal goal)
        {
            if (id != goal.Id)
                return BadRequest();

            _context.Entry(goal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Goals.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>Elimina una meta por ID.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
                return NotFound();

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}