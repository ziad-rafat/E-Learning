using E_Learning.Dtos.Category;
using E_Learning.Dtos.ViewResult;

namespace E_Learning.Application.IService
{
    public interface ICategoryService
    {
        Task<ResultView<CategoryDto>> CreateCategory(CategoryDto categoryDto);
        Task<ResultDataList<CategoryDto>> GetAllCategory();
        Task<ResultView<CategoryDto>> GetCategoryById(Guid id);
        Task<ResultView<CategoryDto>> HardDeleteCategory(CategoryDto category);
        Task<ResultView<CategoryDto>> SoftDeleteCategory(Guid categoryId);
        Task<ResultView<CategoryDto>> UpdateCategory(CategoryDto updatedCategory);
    }
}