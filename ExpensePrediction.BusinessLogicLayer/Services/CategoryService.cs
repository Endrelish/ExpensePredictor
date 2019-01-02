using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects.Category;
using ExpensePrediction.Exceptions;
using ApplicationException = ExpensePrediction.Exceptions.ApplicationException;

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

            try
            {
                if (await _categoryRepository.SaveAsync() > 0)
                {
                    return _mapper.Map<CategoryDto>(category);
                }
                throw new CategoryException("Cannot add category", 500);
            }
            catch (RepositoryException e)
            {
                throw new CategoryException("Cannot add category", e, 500);
            }
        }

        public async Task<CategoryDto> EditCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<TCategory>(categoryDto);
            _categoryRepository.Update(category);

            try
            {
                if (await _categoryRepository.SaveAsync() > 0)
                {
                    return _mapper.Map<CategoryDto>(category);
                }
                throw new CategoryException("Cannot add category", 500);
            }
            catch (RepositoryException e)
            {
                throw new CategoryException("Cannot add category", e, 500);
            }
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