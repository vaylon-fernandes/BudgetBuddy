using webapi.Entities;

namespace WebApi.Models
{
    public class AuthenticateResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }



        public AuthenticateResponse(Users user, string token)
        {
            UserId = user.UserId;
            UserName=user.UserName;
            Email = user.Email;
            Token = token;
        }
    }
}