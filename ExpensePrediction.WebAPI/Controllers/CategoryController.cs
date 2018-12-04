using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        ///     Adds a new category.
        /// </summary>
        /// <param name="categoryType">Type of the category.</param>
        /// <param name="categoryDto">The category data.</param>
        /// <returns>
        ///     Newly created category.
        /// </returns>
        [HttpPost("add/{categoryType}")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("AddCategory")]
        [ProducesResponseType(typeof(CategoryDto), 201)]
        [ProducesResponseType(typeof(string), 400)]
        //TODO Custom exceptions
        public async Task<IActionResult> AddCategory([FromRoute] CategoryType categoryType,
            [FromBody] CategoryDto categoryDto)
        {
            CategoryDto category;
            switch (categoryType)
            {
                case CategoryType.ExpenseCategory:
                    category = await _expenseCategoryService.AddCategoryAsync(categoryDto);
                    break;

                case CategoryType.IncomeCategory:
                    category = await _incomeCategoryService.AddCategoryAsync(categoryDto);
                    break;

                default:
                    category = null;
                    break;
            }

            return CreatedAtRoute("GetCategory", new { CategoryId = category?.Id, CategoryType = categoryType },
                category);
        }

        /// <summary>
        ///     Gets the category.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="categoryType">Type of the category.</param>
        /// <returns>The category.</returns>
        [HttpGet("{categoryType}/{categoryId}", Name = "GetCategory")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("GetCategory")]
        [ProducesResponseType(typeof(CategoryDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 400)]
        //TODO Custom exceptions
        public async Task<IActionResult> GetCategory([FromRoute] string categoryId,
            [FromRoute] CategoryType categoryType)
        {
            CategoryDto category = null;
            switch (categoryType)
            {
                case CategoryType.ExpenseCategory:
                    category = await _expenseCategoryService.GetCategoryAsync(categoryId);
                    break;

                case CategoryType.IncomeCategory:
                    category = await _incomeCategoryService.GetCategoryAsync(categoryId);
                    break;
            }

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        ///     Edits the category.
        /// </summary>
        /// <param name="categoryType">Type of the category.</param>
        /// <param name="categoryDto">The category data.</param>
        /// <returns>The edited category.</returns>
        [HttpPost("edit/{categoryType}")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("EditCategory")]
        [ProducesResponseType(typeof(CategoryDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        //TODO Custom exceptions
        public async Task<IActionResult> EditCategory([FromRoute] CategoryType categoryType,
            [FromBody] CategoryDto categoryDto)
        {
            CategoryDto category = null;
            switch (categoryType)
            {
                case CategoryType.ExpenseCategory:
                    category = await _expenseCategoryService.EditCategoryAsync(categoryDto);
                    break;

                case CategoryType.IncomeCategory:
                    category = await _incomeCategoryService.EditCategoryAsync(categoryDto);
                    break;
            }

            return Ok(category);
        }

        /// <summary>
        ///     Gets all categories of given type.
        /// </summary>
        /// <param name="categoryType">Type of the category.</param>
        /// <returns>All categories.</returns>
        [HttpGet("{categoryType}")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [Authorize("GetCategories")]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        //TODO Custom exceptions
        public async Task<IActionResult> GetCategories([FromRoute] CategoryType categoryType)
        {
            IEnumerable<CategoryDto> categories = null;
            switch (categoryType)
            {
                case CategoryType.ExpenseCategory:
                    categories = await _expenseCategoryService.GetCategoriesAsync();
                    break;

                case CategoryType.IncomeCategory:
                    categories = await _incomeCategoryService.GetCategoriesAsync();
                    break;

                case CategoryType.Undefined:
                    return StatusCode(400, "UNDEFINED_CATEGORY"); //TODO dto with error code and description
            }

            return Ok(categories);
        }
    }
}