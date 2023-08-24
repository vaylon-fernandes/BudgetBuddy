using webapi.Entities;

namespace WebApi.Models
{
    public class AuthenticateResponse
    {
        //public int UserId { get; set; }
        //public string UserName { get; set; }
        //public string Email { get; set; }
        //public string Token { get; set; }
        //public int ExpireDate { get; set; }
        public bool Success { get; set; }
        public List<string> MessageList { get; set; }
        public AuthenticateResponse()
        {
            MessageList = new List<string>();
        }
        
        public AuthenticateResponse(Users user, string token)
        {
            //UserId = user.UserId;
            //UserName = user.UserName;
            //Email = user.Email;
            //Token = token;
        }
    }
}