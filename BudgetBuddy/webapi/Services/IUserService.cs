using webapi.Entities;
using WebApi.Models;

namespace webapi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Users> GetAll();
        Users GetById(int id);
    }

}
