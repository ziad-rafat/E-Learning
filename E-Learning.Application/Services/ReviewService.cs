using AutoMapper;
using E_Learning.Application.Contract;
using E_Learning.Application.IService;
using E_Learning.Application.Mapper;
using E_Learning.Dtos.Category;
using E_Learning.Dtos.Review;
using E_Learning.Dtos.ViewResult;
using E_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.Services
{
    public class ReviewService : IReviewService
    {
        private IReviewRepository _reviewRepository;
        private IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository , IMapper mapper) {

            _reviewRepository = reviewRepository;
            _mapper = mapper;


        }  

        public async Task<ResultView<ReviewDTO>> AddReview(ReviewDTO reviewDTO)
        {
           
            var Review = _mapper.Map<Review>(reviewDTO);
            var NewReview = await _reviewRepository.CreateAsync(Review);
            await _reviewRepository.SaveChangesAsync();
            var CreatedReview = _mapper.Map<ReviewDTO>(NewReview);
            return new ResultView<ReviewDTO> { Entity = CreatedReview, IsSuccess = true, Message = "Created Successfully" };


        }

   
        public async Task<ResultView<ReviewDTO>> UpdateReview(ReviewDTO reviewDTO)
        {
            var ExistingReview = await _reviewRepository.GetByIdAsync(reviewDTO.id);
            if (ExistingReview == null)
            {
                return new ResultView<ReviewDTO>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "Category not found"
                };
            }
            else
            {
                var Review = _mapper.Map<Review>(reviewDTO);
                var AfterUpdate = await _reviewRepository.UpdateAsync(Review);
                await _reviewRepository.SaveChangesAsync();
                var Updated = _mapper.Map<ReviewDTO>(AfterUpdate);
                return new ResultView<ReviewDTO> { Entity = Updated, IsSuccess = true, Message = "Updated Successfully" };
            }
        }


        public async Task<ResultView<ReviewDTO>> HardDelete(Guid reviewid)
        {
            
            var Review = await _reviewRepository.GetByIdAsync(reviewid);
            if(Review != null)
            {
              var deleted=   await  _reviewRepository.DeleteAsync(Review);
              await _reviewRepository.SaveChangesAsync();
              var deletedReview = _mapper.Map<ReviewDTO>(deleted);
                return new ResultView<ReviewDTO> { Entity = deletedReview, IsSuccess = true, Message = "Updated Successfully" };
            }

            else
            {
                return new ResultView<ReviewDTO> { Entity = null, IsSuccess = false, Message = "Updated Failed" };
            }

        }

        public async Task<ResultView<ReviewDTO>> SoftDelete(Guid reviewid)
        {
            var Review = await _reviewRepository.GetByIdAsync(reviewid);
            if(Review != null)
            {
                var deleted = await _reviewRepository.DeleteAsync(Review);
                await _reviewRepository.SaveChangesAsync();
                var deletedReview = _mapper.Map<ReviewDTO>(deleted);
                return new ResultView<ReviewDTO> { Entity = deletedReview, IsSuccess = true, Message = "Updated Successfully" };

            }
            else
            {
                return new ResultView<ReviewDTO> { Entity = null, IsSuccess = false, Message = "Updated Failed" };

            }
        }

        public async Task<ResultView<ReviewDTO>> GetAllReview()
        {
           var reviews = await _reviewRepository.GetAllAsync();
           var Allreviews = _mapper.Map<ReviewDTO>(reviews);
            return new ResultView<ReviewDTO> { Entity = Allreviews, IsSuccess = true, Message = "All Reviews" };

        }

        public async Task<ResultView<ReviewDTO>> GetAllReviewPagination(int iteams , int Count)
        {
            var AllReviews = await _reviewRepository.GetAllAsync();
            var Reviewspiginted  = AllReviews.Skip((iteams - 1)* Count).Take(iteams).ToList();
            var Reviews = _mapper.Map<ReviewDTO>(Reviewspiginted);
            return new ResultView<ReviewDTO> { Entity = Reviews, IsSuccess = true, Message = "Reviews piginted done" };

        }
    }
}
