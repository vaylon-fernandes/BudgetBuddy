using AutoMapper;
using webapi.DTO;
using webapi.Entities;

namespace webapi.Helpers
{
    public class AppMapperProfile:Profile
    {
        public AppMapperProfile() {
            CreateMap<Users, UserDTO>();
            CreateMap<Users, UserExpenseDTO>();
            CreateMap<Expenses, ExpenseDTO>();
        }
    }
}
