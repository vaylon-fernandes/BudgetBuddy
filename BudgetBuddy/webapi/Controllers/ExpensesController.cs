using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Entities;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ExpensesController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetUserExpenses(int id)
        {
          if (_context.Expenses == null)
          {
              return NotFound();
          }
            var userExpenses =  _context.Expenses.Where(user=>user.UserId==id).ToList();
            if(userExpenses==null || userExpenses.Count==0) { return NotFound(); }
            return Ok(userExpenses);
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expenses>> GetExpenses(int id)
        {
          if (_context.Expenses == null)
          {
              return NotFound();
          }
            var expenses = await _context.Expenses.FindAsync(id);

            if (expenses == null)
            {
                return NotFound();
            }

            return expenses;
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenses(int id, Expenses expenses)
        {
            if (id != expenses.ExpenseId)
            {
                return BadRequest();
            }

            _context.Entry(expenses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpensesExists(id))
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

        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expenses>> PostExpenses(Expenses expenses)
        {
          if (_context.Expenses == null)
          {
              return Problem("Entity set 'ApiDbContext.Expenses'  is null.");
          }
            _context.Expenses.Add(expenses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenses", new { id = expenses.ExpenseId }, expenses);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenses(int id)
        {
            if (_context.Expenses == null)
            {
                return NotFound();
            }
            var expenses = await _context.Expenses.FindAsync(id);
            if (expenses == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expenses);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpensesExists(int id)
        {
            return (_context.Expenses?.Any(e => e.ExpenseId == id)).GetValueOrDefault();
        }
    }
}
