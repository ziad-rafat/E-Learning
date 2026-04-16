using AutoMapper;
using E_Learning.Application.Contract;
using E_Learning.Application.IService;
using E_Learning.Dtos.SubCategory;
using E_Learning.Dtos.ViewResult;
using E_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IMapper _mapper;

        public SubCategoryService(ISubCategoryRepository subCategory, IMapper mapper)
        {
            _subCategoryRepository = subCategory;
            _mapper = mapper;
        }

        public async Task<ResultView<SubCategoryDto>> CreateAsync(SubCategoryDto subCategoryDto)
        {
            ResultView<SubCategoryDto> result = new();
            var subCategories = await _subCategoryRepository.GetAllAsync();
            var oldSubCategory = subCategories.FirstOrDefault(sub => sub.Name == subCategoryDto.Name);
            if (oldSubCategory != null)
            {
                result.IsSuccess = false;
                result.Entity = null;
                result.Message = "Aleardy Exist";
                return result;
            }
            var subCategory = _mapper.Map<SubCategory>(subCategoryDto);
            var newSubCategory = await _subCategoryRepository.CreateAsync(subCategory);
            await _subCategoryRepository.SaveChangesAsync();
            var createdSubCategory = _mapper.Map<SubCategoryDto>(newSubCategory);

            result.IsSuccess = true; result.Entity = createdSubCategory; result.Message = "Created Successfully";
            return result;
        }

        public async Task<ResultView<SubCategoryDto>> UpdateAsync(SubCategoryDto updatedSubCategoryDto)
        {
            var existingSubCategory = await _subCategoryRepository.GetByIdAsync(updatedSubCategoryDto.Id);

            if (existingSubCategory == null)
            {
                return new ResultView<SubCategoryDto>
                {
                    IsSuccess = false,
                    Entity = null,
                    Message = "Subcategory not found"
                };
            }

            _mapper.Map(updatedSubCategoryDto, existingSubCategory);
            await _subCategoryRepository.UpdateAsync(existingSubCategory);
            await _subCategoryRepository.SaveChangesAsync();

            var updatedSubCategory = _mapper.Map<SubCategoryDto>(existingSubCategory);
            return new ResultView<SubCategoryDto>
            {
                IsSuccess = true,
                Entity = updatedSubCategory,
                Message = "Updated Successfully"
            };
        }

        public async Task<ResultView<SubCategoryDto>> HardDeleteAsync(SubCategoryDto subCategoryDto)
        {
            try
            {
                var existingSubCategory = await _subCategoryRepository.GetByIdAsync(subCategoryDto.Id);

                if (existingSubCategory == null)
                {
                    return new ResultView<SubCategoryDto>
                    {
                        IsSuccess = false,
                        Entity = null,
                        Message = "Subcategory not found"
                    };
                }

                var mappedSubCategory = _mapper.Map<SubCategory>(existingSubCategory);
                await _subCategoryRepository.DeleteAsync(mappedSubCategory);
                await _subCategoryRepository.SaveChangesAsync();

                return new ResultView<SubCategoryDto>
                {
                    IsSuccess = true,
                    Entity = _mapper.Map<SubCategoryDto>(mappedSubCategory),
                    Message = "Deleted Successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResultView<SubCategoryDto>
                {
                    IsSuccess = false,
                    Entity = null,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultView<SubCategoryDto>> SoftDeleteAsync(Guid subCategoryId)
        {
            try
            {
                var subCategory = await _subCategoryRepository.GetByIdAsync(subCategoryId);
                if (subCategory == null)
                {
                    return new ResultView<SubCategoryDto>
                    {
                        IsSuccess = false,
                        Entity = null,
                        Message = "Subcategory not found"
                    };
                }

                subCategory.IsDeleted = true;
                await _subCategoryRepository.SaveChangesAsync();
                var deletedSubCategory = _mapper.Map<SubCategoryDto>(subCategory);
                return new ResultView<SubCategoryDto>
                {
                    IsSuccess = true,
                    Entity = deletedSubCategory,
                    Message = "Deleted Successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResultView<SubCategoryDto>
                {
                    IsSuccess = false,
                    Entity = null,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultDataList<SubCategoryDto>> GetAllAsync()
        {
            var subCategories = await _subCategoryRepository.GetAllAsync();
            var subCategoryDtos = subCategories.Where(s => s.IsDeleted == false)
                                               .Select(s => _mapper.Map<SubCategoryDto>(s))
                                               .ToList();

            return new ResultDataList<SubCategoryDto>
            {
                Entities = subCategoryDtos,
                Count = subCategoryDtos.Count
            };
        }

        public async Task<ResultView<SubCategoryDto>> GetByIdAsync(Guid id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id);
            if (subCategory == null)
            {
                return new ResultView<SubCategoryDto>
                {
                    IsSuccess = false,
                    Entity = null,
                    Message = "Subcategory not found"
                };
            }

            return new ResultView<SubCategoryDto>
            {
                IsSuccess = true,
                Entity = _mapper.Map<SubCategoryDto>(subCategory),
                Message = "Subcategory retrieved successfully"
            };
        }
    }
}
