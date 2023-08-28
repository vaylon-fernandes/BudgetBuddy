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
            CreateMap<ExpenseDTO, Expenses>();
            CreateMap<Budget, BudgetDTO>();
            CreateMap<Savings, SavingsDTO>();
            CreateMap<SavingsDTO, Savings>();

        }
    }
}
