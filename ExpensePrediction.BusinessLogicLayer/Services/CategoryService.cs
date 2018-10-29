using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.DataTransferObjects.Category;

namespace ExpensePrediction.BusinessLogicLayer.Services
{
    public class CategoryService<TCategory> : ICategoryService<TCategory> where TCategory : Category
    {
        private readonly IApplicationRepository<TCategory> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IApplicationRepository<TCategory> categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<TCategory> AddCategory(CategoryDto categoryDto)
        {
            categoryDto.Id = string.Empty;
            var category = _mapper.Map<TCategory>(categoryDto);
            await _categoryRepository.CreateAsync(category);

            if (await _categoryRepository.SaveAsync() > 0)
                return category;
            throw new Exception("nie da sie"); //TODO custom exception
        }

        public Task<TCategory> EditCategory(CategoryDto categoryDto)
        {

        }

        public Task<IEnumerable<TCategory>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<TCategory> GetCategory(string categoryId) => _categoryRepository.FindByIdAsync(categoryId);
    }
}