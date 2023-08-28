using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Data;
using webapi.Entities;
//using webapi.Services;
using WebApi.Models;
using webapi.Utils;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using webapi.DTO;
using Humanizer;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;
       // private IUserService _userService;
        private IConfiguration _configuration;
        private IMapper _mapper;

        public UserController(ApiDbContext dbContext, IConfiguration configuration, IMapper mapper)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Users>>> GetAll()
        {
            var users = _dbContext.User;
           var requieredInfo = _mapper.Map<List<UserDTO>>(users);

            return Ok(requieredInfo);   
        }


        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<Users>> GetUserById(int userId) {
            //var user = await _dbContext.User.FindAsync(userId);
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("user not found");
            }
            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.UserId)
            {
                return BadRequest();
            }

            _dbContext.Entry(users).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        
        [Authorize]
        [HttpDelete("{userId:int}")]
        public async Task<ActionResult<Users>> DeleteUser(int userId)
        {
            using (var dbContext = new ApiDbContext())
            {
                try
                {
                    var user = await _dbContext.User.FindAsync(userId);
                    _dbContext.User.Remove(user);
                }
                catch {
                    return BadRequest("Invalid User Id");
                }
                await _dbContext.SaveChangesAsync();

            }

            return NoContent();
        }

        // User expense methods
        [HttpGet("AllExpenses")]
        public async Task<IActionResult> GetUserWithExpenses()
        {
            var users = await _dbContext.User
            .Include(_ => _.Expenses).ToListAsync();
            var requieredInfo = _mapper.Map<List<UserExpenseDTO>>(users);

            return Ok(requieredInfo);
        }

        // GET: api/Users/5/Expenses
        [Authorize]
        [HttpGet("{id}/GetExpenses")]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetUserWithExpensesById(int id)
        {
            //List<Expenses> expenses = await _dbContext.Expenses.Where(e => e.UserId == id).ToListAsync();

            var expensesById = await _dbContext.User
            .Include(_ => _.Expenses).Where(_ => _.UserId == id).ToListAsync();


            if (expensesById == null)
            {
                return NotFound();
            }

            return Ok(expensesById);
        }

        // user budget methods 
        [Authorize]
        [HttpGet("{userId:int}/Budget")]
        public async Task<ActionResult<BudgetDTO>> GetUserBudget(int userId)
        {
            var user = await _dbContext.User
                .Include(u => u.Budget) // Include the Budget navigation property
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var budgetDto = _mapper.Map<BudgetDTO>(user.Budget);

            return Ok(budgetDto);
        }

        [Authorize]
        [HttpPost("{userId:int}/Budget")]
        public async Task<IActionResult> CreateOrUpdateUserBudget(int userId, BudgetDTO budgetDto)
        {
            var user = await _dbContext.User.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var budget = _mapper.Map<Budget>(budgetDto);
            budget.UserId = userId;

            if (user.Budget == null)
            {
                // Create a new Budget record
                user.Budget = budget;
            }
            else
            {
                // Update the existing Budget record
                _mapper.Map(budget, user.Budget);
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // GET: api/Users/5/FinancialGoals
        [Authorize]
        [HttpGet("{userId:int}/GetFinancialGoals")]
        public async Task<ActionResult<IEnumerable<FinancialGoal>>> GetUserWithFinancialGoalsById(int userId)
        {
            var financialGoalsById = await _dbContext.FinancialGoals
                .Where(goal => goal.UserId == userId)
                .ToListAsync();

            return Ok(financialGoalsById);
        }

        // POST: api/Users/5/AddFinancialGoal
        [Authorize]
        [HttpPost("{userId:int}/AddFinancialGoal")]
        public async Task<ActionResult<FinancialGoal>> AddFinancialGoalForUser(int userId, FinancialGoal financialGoal)
        {
            financialGoal.UserId = userId; // Set the UserId explicitly

            _dbContext.FinancialGoals.Add(financialGoal);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserWithFinancialGoalsById), new { userId = financialGoal.UserId }, financialGoal);
        }

        // PUT: api/Users/5/UpdateFinancialGoal/1
        [Authorize]
        [HttpPut("{userId:int}/UpdateFinancialGoal/{goalId:int}")]
        public async Task<IActionResult> UpdateFinancialGoalForUser(int userId, int goalId, FinancialGoal financialGoal)
        {
            if (userId != financialGoal.UserId || goalId != financialGoal.GoalId)
            {
                return BadRequest("Invalid userId or goalId in the request.");
            }

            _dbContext.Entry(financialGoal).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinancialGoalExists(goalId))
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

        // DELETE: api/Users/5/DeleteFinancialGoal/1
        [Authorize]
        [HttpDelete("{userId:int}/DeleteFinancialGoal/{goalId:int}")]
        public async Task<ActionResult<FinancialGoal>> DeleteFinancialGoalForUser(int userId, int goalId)
        {
            var financialGoal = await _dbContext.FinancialGoals.FindAsync(goalId);

            if (financialGoal == null || financialGoal.UserId != userId)
            {
                return NotFound();
            }

            _dbContext.FinancialGoals.Remove(financialGoal);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // Savings methods

        // GET: api/Users/5/Savings
        [Authorize]
        [HttpGet("{userId:int}/GetSavings")]
        public async Task<ActionResult<SavingsDTO>> GetUserSavings(int userId)
        {
            var savings = await _dbContext.Savings
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (savings == null)
            {
                return NotFound("Savings not found for the user.");
            }

            var savingsDTO = _mapper.Map<SavingsDTO>(savings);
            return Ok(savingsDTO);
        }

        // POST: api/Users/5/AddSavings
        [Authorize]
        [HttpPost("{userId:int}/AddSavings")]
        public async Task<ActionResult<SavingsDTO>> AddSavingsForUser(int userId, SavingsDTO savingsDTO)
        {
            var savings = _mapper.Map<Savings>(savingsDTO);
            savings.UserId = userId; // Set the UserId explicitly

            _dbContext.Savings.Add(savings);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserSavings), new { userId }, savingsDTO);
        }

        // PUT: api/Users/5/UpdateSavings
        [Authorize]
        [HttpPut("{userId:int}/UpdateSavings")]
        public async Task<IActionResult> UpdateSavingsForUser(int userId, SavingsDTO savingsDTO)
        {
            var existingSavings = await _dbContext.Savings
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (existingSavings == null)
            {
                return NotFound("Savings not found for the user.");
            }

            var updatedSavings = _mapper.Map(savingsDTO, existingSavings);

            _dbContext.Entry(updatedSavings).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Users/5/DeleteSavings
        [Authorize]
        [HttpDelete("{userId:int}/DeleteSavings")]
        public async Task<ActionResult<SavingsDTO>> DeleteSavingsForUser(int userId)
        {
            var savings = await _dbContext.Savings
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (savings == null)
            {
                return NotFound("Savings not found for the user.");
            }

            _dbContext.Savings.Remove(savings);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // helper methods
        private bool UsersExists(int id)
        {
            return (_dbContext.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        private bool ExpensesExists(int id)
        {
            return (_dbContext.Expenses?.Any(e => e.ExpenseId == id)).GetValueOrDefault();
        }

        private bool FinancialGoalExists(int id)
        {
            return _dbContext.FinancialGoals.Any(e => e.GoalId == id);
        }


    }

}
