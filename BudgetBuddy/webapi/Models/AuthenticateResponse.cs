using webapi.Entities;

namespace WebApi.Models
{
    public class AuthenticateResponse
    {
        
        public bool Success { get; set; }
        public List<string> MessageList { get; set; }
        public AuthenticateResponse()
        {
            MessageList = new List<string>();
        }
        
        
    }
}