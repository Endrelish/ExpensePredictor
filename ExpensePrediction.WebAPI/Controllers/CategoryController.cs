using System;
using System.Net.Mime;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.DataTransferObjects.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService<IncomeCategory> _incomeCategoryService;
        private readonly ICategoryService<ExpenseCategory> _expenseCategoryService;
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper, ICategoryService<ExpenseCategory> expenseCategoryService, ICategoryService<IncomeCategory> incomeCategoryService)
        {
            _mapper = mapper;
            _expenseCategoryService = expenseCategoryService;
            _incomeCategoryService = incomeCategoryService;
        }

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="categoryType">Type of the category.</param>
        /// <param name="categoryDto">The category data.</param>
        /// <returns>Newly created category.</returns>
        [HttpPost("add/{categoryType}")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("AddCategory")]
        public async Task<IActionResult> AddCategory([FromRoute] CategoryType categoryType, [FromBody] CategoryDto categoryDto)
        {
            Category category;
            switch (categoryType)
            {
                case CategoryType.ExpenseCategory:
                    category = await _expenseCategoryService.AddCategory(categoryDto);
                    break;
                case CategoryType.IncomeCategory:
                    category = await _incomeCategoryService.AddCategory(categoryDto);
                    break;
                default:
                    category = null;
                    break;
            }

            return CreatedAtRoute("GetCategory", new {CategoryId = category?.Id, CategoryType = categoryType}, _mapper.Map<CategoryDto>(category));
        }

        [HttpGet("{categoryType}/{categoryId}", Name = "GetCategory")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("GetCategory")]
        public async Task<IActionResult> GetCategory([FromRoute] string categoryId, [FromRoute] CategoryType categoryType)
        {
            Category category;
            switch (categoryType)
            {
                case CategoryType.ExpenseCategory:
                    category = await _expenseCategoryService.GetCategory(categoryId);
                    break;
                case CategoryType.IncomeCategory:
                    category = await _expenseCategoryService.GetCategory(categoryId);
                    break;
                default:
                    category = null;
                    break;
            }

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        [HttpPost("edit/{categoryType}")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("EditCategory")]
        public async Task<IActionResult> EditCategory([FromBody] CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }

        //GetCategories
    }
}