using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.DataTransferObjects.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService<ExpenseCategory> _expenseCategoryService;
        private readonly ICategoryService<IncomeCategory> _incomeCategoryService;

        public CategoryController(ICategoryService<ExpenseCategory> expenseCategoryService,
            ICategoryService<IncomeCategory> incomeCategoryService)
        {
            _expenseCategoryService = expenseCategoryService;
            _incomeCategoryService = incomeCategoryService;
        }

        /// <summary>
        ///     Adds the category.
        /// </summary>
        /// <param name="categoryType">Type of the category.</param>
        /// <param name="categoryDto">The category data.</param>
        /// <returns>Newly created category.</returns>
        [HttpPost("add/{categoryType}")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("AddCategory")]
        public async Task<IActionResult> AddCategory([FromRoute] CategoryType categoryType,
            [FromBody] CategoryDto categoryDto)
        {
            try
            {
                CategoryDto category;
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

                return CreatedAtRoute("GetCategory", new {CategoryId = category?.Id, CategoryType = categoryType},
                    category);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpGet("{categoryType}/{categoryId}", Name = "GetCategory")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("GetCategory")]
        public async Task<IActionResult> GetCategory([FromRoute] string categoryId,
            [FromRoute] CategoryType categoryType)
        {
            try
            {
                CategoryDto category = null;
                switch (categoryType)
                {
                    case CategoryType.ExpenseCategory:
                        category = await _expenseCategoryService.GetCategory(categoryId);
                        break;

                    case CategoryType.IncomeCategory:
                        category = await _incomeCategoryService.GetCategory(categoryId);
                        break;
                }

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception)
            {
                return StatusCode(400, "ERROR");
            }
        }

        [HttpPost("edit/{categoryType}")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("EditCategory")]
        public async Task<IActionResult> EditCategory([FromRoute] CategoryType categoryType,
            [FromBody] CategoryDto categoryDto)
        {
            try
            {
                CategoryDto category = null;
                switch (categoryType)
                {
                    case CategoryType.ExpenseCategory:
                        category = await _expenseCategoryService.EditCategory(categoryDto);
                        break;

                    case CategoryType.IncomeCategory:
                        category = await _incomeCategoryService.EditCategory(categoryDto);
                        break;
                }

                return Ok(category);
            }
            catch (Exception)
            {
                return StatusCode(400, "ERROR");
            }
        }

        [HttpGet("{categoryType}")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("GetCategories")]
        public async Task<IActionResult> GetCategories([FromRoute] CategoryType categoryType)
        {
            try
            {
                IEnumerable<CategoryDto> categories = null;
                switch (categoryType)
                {
                    case CategoryType.ExpenseCategory:
                        categories = await _expenseCategoryService.GetCategories();
                        break;

                    case CategoryType.IncomeCategory:
                        categories = await _incomeCategoryService.GetCategories();
                        break;
                    case CategoryType.Undefined:
                        return StatusCode(400, "UNDEFINED_CATEGORY"); //TODO dto with error code and description
                }

                return Ok(categories);
            }
            catch (Exception)
            {
                return StatusCode(400, "ERROR");
            }
        }
    }
}