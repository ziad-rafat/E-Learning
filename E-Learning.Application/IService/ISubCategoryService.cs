using E_Learning.Dtos.SubCategory;
using E_Learning.Dtos.ViewResult;

namespace E_Learning.Application.IService
{
    public interface ISubCategoryService
    {
        Task<ResultView<SubCategoryDto>> CreateAsync(SubCategoryDto subCategoryDto);
        Task<ResultDataList<SubCategoryDto>> GetAllAsync();
        Task<ResultView<SubCategoryDto>> GetByIdAsync(Guid id);
        Task<ResultView<SubCategoryDto>> HardDeleteAsync(SubCategoryDto subCategoryDto);
        Task<ResultView<SubCategoryDto>> SoftDeleteAsync(Guid subCategoryId);
        Task<ResultView<SubCategoryDto>> UpdateAsync(SubCategoryDto updatedSubCategoryDto);
    }
}