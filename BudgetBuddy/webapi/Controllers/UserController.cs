using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Entities;
using webapi.Services;
using WebApi.Models;


namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;
        private IUserService _userService;

        // private readonly ILogger<UserController> _logger;

        /*public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        */
        public UserController(ApiDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

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
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<Users>> GetUserById(int userId) {
            var user = await _dbContext.User.FindAsync(userId);
            return Ok(user);
        }
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
