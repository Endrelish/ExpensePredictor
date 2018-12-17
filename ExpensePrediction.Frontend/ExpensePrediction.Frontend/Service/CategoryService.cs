using ExpensePrediction.DataTransferObjects.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.Frontend.Service
{
    class CategoryService
    {
        public async Task<List<CategoryDto>> GetCategories(CategoryType type)
        {
            return await RestService.GetAsync<List<CategoryDto>>(Constants.GetCategoriesUri(type));
        }
    }
}
