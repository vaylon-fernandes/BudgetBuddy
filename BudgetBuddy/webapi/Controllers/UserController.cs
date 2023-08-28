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

<<<<<<< HEAD
        /*[HttpGet]
        public ActionResult<IEnumerable<Users>> GetAllUsers()
        {
            return _dbContext.User;
        }*/

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult<LoginResponse> Login(AuthenticateRequest requestLogin)
        {
            var responseLogin = new LoginResponse();
            using (var db = _dbContext)
            {
                var existingUser = db.User.SingleOrDefault(x => x.Email == requestLogin.Email);
                if (existingUser != null)
                {
                    var isPasswordVerified = CryptoUtil.VerifyPassword(requestLogin.Password, existingUser.Salt, existingUser.Password);
                    if (isPasswordVerified)
                    {
                        var claimList = new List<Claim>();
                        claimList.Add(new Claim(ClaimTypes.Name, existingUser.Email));
                        claimList.Add(new Claim(ClaimTypes.Role, existingUser.UserRole.ToString()));
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var expireDate = DateTime.UtcNow.AddDays(1);
                        var timeStamp = DateUtil.ConvertToTimeStamp(expireDate);
                        var token = new JwtSecurityToken(
                            claims: claimList,
                            notBefore: DateTime.UtcNow,
                            expires: expireDate,
                            signingCredentials: creds);
                        responseLogin.Success = true;
                        responseLogin.Email = requestLogin.Email;
                        responseLogin.Token = new JwtSecurityTokenHandler().WriteToken(token);
                        responseLogin.ExpireDate = timeStamp;

                    }
                    else
                    {
                        responseLogin.Success = false;
                        responseLogin.MessageList.Add("Password is wrong");
                    }
                }
                else
                {
                    responseLogin.Success = false;
                    responseLogin.MessageList.Add("Email is wrong");
                }
            }
            return responseLogin;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public ActionResult<AuthenticateResponse> Register(AuthenticateRequest requestRegister)
        {
            var responseRegister = new AuthenticateResponse();
            using (var db = new ApiDbContext())
            {
                if (!db.User.Any(x => x.Email == requestRegister.Email))
                {
                    var email = requestRegister.Email;
                    var salt = CryptoUtil.GenerateSalt();
                    var password = requestRegister.Password;
                    var hashedPassword = CryptoUtil.HashMultiple(password, salt);
                    var user = new Users();
                    user.Email = email;
                    user.Salt = salt;
                    user.Password = hashedPassword;
                    user.UserRole = Role.USER;
                    db.User.Add(user);
                    db.SaveChanges();
                    responseRegister.Success = true;
                    Console.WriteLine($"{email} {password}");
                }
                else
                {
                    responseRegister.MessageList.Add("Email is already in use");
                }
            }
            return responseRegister;
        }
=======
>>>>>>> Development

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
