using System.Threading.Tasks;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.DataTransferObjects.Category;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface ICategoryService<TCategory> where TCategory : Category
    {
        Task<TCategory> AddCategory(CategoryDto categoryDto);
        Task<TCategory> GetCategory(string categoryId);
    }
}