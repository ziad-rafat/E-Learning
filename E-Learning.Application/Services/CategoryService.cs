using AutoMapper;
using E_Learning.Application.Contract;
using E_Learning.Application.IService;
using E_Learning.Dtos.Category;
using E_Learning.Dtos.ViewResult;
using E_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ResultView<CategoryDto>> CreateCategory(CategoryDto categoryDto)
        {
            var allcategories = await _categoryRepository.GetAllAsync();
            var oldcat = allcategories.Where(c => c.Name == categoryDto.Name).FirstOrDefault();
            if (oldcat != null)
            {
                return new ResultView<CategoryDto> { Entity = null, IsSuccess = false, Message = "Category Already Exists" };
            }
            else
            {
                var category = _mapper.Map<Category>(categoryDto);
                var newCategory = await _categoryRepository.CreateAsync(category);
                await _categoryRepository.SaveChangesAsync();
                var createdCategory = _mapper.Map<CategoryDto>(newCategory);
                return new ResultView<CategoryDto> { Entity = createdCategory, IsSuccess = true, Message = "Created Successfully" };
            }
        }

        public async Task<ResultView<CategoryDto>> UpdateCategory(CategoryDto updatedCategory)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(updatedCategory.Id);

            if (existingCategory == null)
            {
                return new ResultView<CategoryDto>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "Category not found"
                };
            }

            // Map updated values to the existing entity
            _mapper.Map(updatedCategory, existingCategory);

            // Mark the entity as modified
            await _categoryRepository.UpdateAsync(existingCategory);
            await _categoryRepository.SaveChangesAsync();
            var categoryDto = _mapper.Map<CategoryDto>(existingCategory);

            return new ResultView<CategoryDto>
            {
                Entity = categoryDto,
                IsSuccess = true,
                Message = "Updated Successfully"
            };
        }

        public async Task<ResultView<CategoryDto>> HardDeleteCategory(CategoryDto category)
        {
            try
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(category.Id);
                var modelCategory = _mapper.Map<Category>(existingCategory);
                var oldCategory = _categoryRepository.DeleteAsync(modelCategory);
                await _categoryRepository.SaveChangesAsync();
                var categoryDto = _mapper.Map<CategoryDto>(oldCategory);
                return new ResultView<CategoryDto> { Entity = categoryDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<CategoryDto> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }
        public async Task<ResultView<CategoryDto>> SoftDeleteCategory(Guid categoryId)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                var modelCat = _mapper.Map<Category>(category);
                var oldCategory = (await _categoryRepository.GetAllAsync()).FirstOrDefault(b => b.Id == category.Id);
                oldCategory.IsDeleted = true;
                await _categoryRepository.SaveChangesAsync();

                var categoryDto = _mapper.Map<CategoryDto>(oldCategory);
                return new ResultView<CategoryDto> { Entity = categoryDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<CategoryDto> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }
        public async Task<ResultDataList<CategoryDto>> GetAllCategory()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var categoryDtos = categories.Where(c => c.IsDeleted == false).Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Ar_Name = c.Ar_Name,
            }).ToList();

            ResultDataList<CategoryDto> result = new ResultDataList<CategoryDto>();
            result.Entities = categoryDtos;
            result.Count = categoryDtos.Count();
            return result;
        }
       
        public async Task<ResultView<CategoryDto>> GetCategoryById(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new ResultView<CategoryDto> { Entity = categoryDto, IsSuccess = true , Message = "Category Found" };
        }
    }
}
