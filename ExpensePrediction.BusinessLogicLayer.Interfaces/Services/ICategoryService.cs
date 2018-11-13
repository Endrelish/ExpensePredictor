using System.Collections.Generic;
using System.Threading.Tasks;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects.Category;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface ICategoryService<TCategory> where TCategory : Category
    {
        Task<CategoryDto> AddCategoryAsync(CategoryDto categoryDto);
        Task<CategoryDto> GetCategoryAsync(string categoryId);
        Task<CategoryDto> EditCategoryAsync(CategoryDto categoryDto);
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    }
}