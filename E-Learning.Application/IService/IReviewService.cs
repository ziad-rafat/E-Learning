using E_Learning.Dtos.Review;
using E_Learning.Dtos.ViewResult;
using E_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.IService
{
    public interface IReviewService
    {
        Task<ResultView<ReviewDTO>> AddReview(ReviewDTO reviewDTO);
        Task<ResultView<ReviewDTO>> UpdateReview(ReviewDTO reviewDTO);
        Task<ResultView<ReviewDTO>> HardDelete(Guid reviewid);
        Task<ResultView<ReviewDTO>> SoftDelete(Guid reviewid);
        Task<ResultView<ReviewDTO>> GetAllReview();
        Task<ResultView<ReviewDTO>> GetAllReviewPagination(int iteams, int Count);


    }
}
