﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;
        
        public UserController(ApiDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        [HttpGet]
        public ActionResult<IEnumerable<Users>> GetAllUsers()
        {
            return _dbContext.User;
        }
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<Users>> GetUserById(int userId) {
            var user = await _dbContext.User.FindAsync(userId);
            return user;
        }
        [HttpPost]
        public async Task<ActionResult<Users>> RegisterUser(Users user)
        {
            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return Ok();    
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
            return Ok();
        }

    }
}