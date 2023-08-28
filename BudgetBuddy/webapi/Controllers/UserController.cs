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

        // TODO: Include all entities -- Cascade
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

        
        // helper methods
        private bool UsersExists(int id)
        {
            return (_dbContext.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        private bool ExpensesExists(int id)
        {
            return (_dbContext.Expenses?.Any(e => e.ExpenseId == id)).GetValueOrDefault();
        }

    }

}
