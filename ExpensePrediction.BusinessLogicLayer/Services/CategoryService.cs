using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
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

        public async Task<CategoryDto> AddCategoryAsync(CategoryDto categoryDto)
        {
            categoryDto.Id = null;
            var category = _mapper.Map<TCategory>(categoryDto);
            await _categoryRepository.CreateAsync(category);

            if (await _categoryRepository.SaveAsync() > 0)
            {
                return _mapper.Map<CategoryDto>(category);
            }

            throw new Exception("nie da sie"); //TODO custom exceptions
        }

        public async Task<CategoryDto> EditCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<TCategory>(categoryDto);
            _categoryRepository.Update(category);

            if (await _categoryRepository.SaveAsync() > 0)
            {
                return _mapper.Map<CategoryDto>(category);
            }

            throw new Exception(); //TODO custom exceptions
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            return _mapper.Map<IEnumerable<CategoryDto>>(await _categoryRepository.FindAllAsync());
        }

        public async Task<CategoryDto> GetCategoryAsync(string categoryId)
        {
            return _mapper.Map<CategoryDto>(await _categoryRepository.FindByIdAsync(categoryId));
        }
    }
}