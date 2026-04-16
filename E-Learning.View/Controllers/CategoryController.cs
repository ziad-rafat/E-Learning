using E_Learning.Application.IService;
using E_Learning.Application.Services;
using E_Learning.Dtos.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Learning.View.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) { _categoryService = categoryService; }
        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategory();
            return Ok(categories);
        }

        // GET api/<CategoryController>/5
        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetByID(Guid Id)
        {
            var data = await _categoryService.GetCategoryById(Id);
            return Ok(data);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Create( CategoryDto category)
        {
            var result = await _categoryService.CreateCategory(category);
            if (result.IsSuccess)
            {
                return Ok(result.Entity);
            }
            else
            { return BadRequest(result); }
        }

        // PUT api/<CategoryController>/5
        [HttpPut]
        public async Task<IActionResult> Update( CategoryDto category)
        {
            var result = await _categoryService.UpdateCategory(category);
            if (result.IsSuccess)
            {
                return Ok(result.Entity);
            }
            else
            { return BadRequest(result); }
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("DeleteCategory/{categoryId:Guid}")]    
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var data = await _categoryService.SoftDeleteCategory(categoryId);
            return Ok(data);
        }
    }
}
