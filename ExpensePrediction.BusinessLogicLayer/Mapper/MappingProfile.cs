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

            CreateMap<ExpenseDto, Expense>()
                .ForSourceMember(src => src.LinkedExpenseId, opt => opt.Ignore())
                .ForSourceMember(src => src.CategoryId, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.LinkedExpenseId, opt => opt.MapFrom(src => src.LinkedExpense.Id))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id));

            CreateMap<CategoryDto, ExpenseCategory>();
            CreateMap<CategoryDto, IncomeCategory>();
            CreateMap<NewCategoryDto, IncomeCategory>();
            CreateMap<NewCategoryDto, ExpenseCategory>();
        }
    }
}