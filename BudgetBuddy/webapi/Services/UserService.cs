namespace WebApi.Services;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Data;
using webapi.Entities;
using webapi.Services;
using WebApi.Helpers;
using WebApi.Models;


public class UserService : IUserService
{
    private readonly ApiDbContext _dbContext;
    private readonly AppSettings _appSettings;

    public UserService(IOptions<AppSettings> appSettings, ApiDbContext dbContext)
    {
        _appSettings = appSettings.Value;
        _dbContext = dbContext;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _dbContext.User.FirstOrDefault(x => x.UserName == model.Username && x.Password == model.Password);

        //var user = _users.SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password);

        // return null if user not found
        if (user == null) return null;

        // authentication successful so generate jwt token
        var token = generateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public IEnumerable<Users> GetAll()
    {
        return _dbContext.User;
    }

    public Users GetById(int id)
    {
        return _dbContext.Find<Users>(id);
    }

    // helper methods

    private string generateJwtToken(Users user)
    {
        // generate token that is valid for 1 day
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}