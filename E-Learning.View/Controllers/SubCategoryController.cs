using E_Learning.Application.IService;
using E_Learning.Application.Services;
using E_Learning.Dtos.SubCategory;
using E_Learning.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Learning.View.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService) { _subCategoryService = subCategoryService; }
        // GET: api/<SubCategoryController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        { 
            var subCategories = await _subCategoryService.GetAllAsync();
            return Ok(subCategories);
        }

        // GET api/<SubCategoryController>/5
        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetByID(Guid Id)
        {
            var data = await _subCategoryService.GetByIdAsync(Id);
            return Ok(data);
        }

        // POST api/<SubCategoryController>
        [HttpPost]
        public async Task<IActionResult> Create(SubCategoryDto subCategoryDto)
        {
            var result = await _subCategoryService.CreateAsync(subCategoryDto);
            if (result.IsSuccess)
            {
                return Ok(result.Entity);
            }
            else
            { return BadRequest(result); }
        }

        // PUT api/<SubCategoryController>/5
        [HttpPut]
        public async Task<IActionResult> Update(SubCategoryDto subCategoryDto)
        {
            var result = await _subCategoryService.UpdateAsync(subCategoryDto);
            if (result.IsSuccess)
            {
                return Ok(result.Entity);
            }
            else
            { return BadRequest(result); }
        }

        // DELETE api/<SubCategoryController>/5
        [HttpDelete("DeleteCategory/{subCategoryId:Guid}")]
        public async Task<IActionResult> DeleteCategory(Guid subCategoryId)
        {
            var data = await _subCategoryService.SoftDeleteAsync(subCategoryId);
            return Ok(data);
        }
    }
}
