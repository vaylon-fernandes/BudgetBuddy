using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Data;
using webapi.Entities;
using webapi.Services;
using WebApi.Models;
using webapi.Utils;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;
        private IUserService _userService;
        private IConfiguration _configuration;

        public UserController(ApiDbContext dbContext, IUserService userService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(response);
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users =  _userService.GetAll();
            return Ok(users);
        }

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
                        responseLogin.Token = new JwtSecurityTokenHandler().WriteToken(token);
                        responseLogin.ExpireDate = timeStamp;
                    }
                    else
                    {
                        responseLogin.MessageList.Add("Password is wrong");
                    }
                }
                else
                {
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

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<Users>> GetUserById(int userId) {
            var user = await _dbContext.User.FindAsync(userId);
            return Ok(user);
        }
        /*[AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Users>> RegisterUser(Users user)
        {
            try
            {
                await _dbContext.User.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Conflict("Duplicate Record Exists");
            }
            
            return Created("Successfully Registered", user);    
        }
        */
        [HttpPut]
        public async Task<ActionResult<Users>> UpdateUser(Users user)
        {
            _dbContext.User.Update(user); 
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{userId:int}")]
        public async Task<ActionResult<Users>> DeleteUser(int userId)
        {
            var user = await _dbContext.User.FindAsync(userId);
            _dbContext.User.Remove(user);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}
