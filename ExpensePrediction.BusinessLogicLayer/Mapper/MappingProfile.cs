using AutoMapper;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.DataTransferObjects.Category;
using ExpensePrediction.DataTransferObjects.User;

namespace ExpensePrediction.BusinessLogicLayer.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // TODO Create mapping profiles
            // CreateMap<T1, T2>();

            CreateMap<RegisterDto, User>();
            CreateMap<UserDataDto, User>();

            CreateMap<ExpenseDto, Expense>();
            CreateMap<IncomeDto, Income>();

            CreateMap<CategoryDto, ExpenseCategory>();
            CreateMap<CategoryDto, IncomeCategory>();
        }
    }
}