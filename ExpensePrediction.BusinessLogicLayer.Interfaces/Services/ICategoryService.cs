using System.Collections.Generic;
using System.Threading.Tasks;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.DataTransferObjects.Category;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface ICategoryService<TCategory> where TCategory : Category
    {
        Task<CategoryDto> AddCategory(CategoryDto categoryDto);
        Task<CategoryDto> GetCategory(string categoryId);
        Task<CategoryDto> EditCategory(CategoryDto categoryDto);
        Task<IEnumerable<CategoryDto>> GetCategories();
    }
}