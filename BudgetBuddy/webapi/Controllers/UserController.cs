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

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;
       // private IUserService _userService;
        private IConfiguration _configuration;

        public UserController(ApiDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Users>>> GetAll()
        {
            var users = _dbContext.User;
            return Ok(users);   
        }


        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<Users>> GetUserById(int userId) {
            var user = await _dbContext.User.FindAsync(userId);
            if (user == null)
            {
                return NotFound("user not found");
            }
            return Ok(user);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Users>> UpdateUser(Users user)
        {
            using (var dbContext = new ApiDbContext())
            {
                dbContext.Entry(user).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
            return Ok();
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

    }
}
