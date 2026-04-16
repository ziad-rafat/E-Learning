using E_Learning.Dtos.Course;
using E_Learning.Dtos.ViewResult;

namespace E_Learning.Application.IService
{
    public interface ICourseService
    {
        Task<ResultView<CreateOrUpdateCourseDTO>> CreateCourseAsync(CreateOrUpdateCourseDTO courseDto);
        Task<ResultView<CreateOrUpdateCourseDTO>> UpdateCourseAsync(CreateOrUpdateCourseDTO courseDto);
        Task<ResultView<CourseDTO>> GetCourseAsync(Guid courseId);
        Task<ResultDataList<CourseListDTO>> GetAllCoursesAsync();
        Task<ResultDataList<CourseListDTO>> GetAllCoursesAsync(CourseSearchDTO parameters);
        Task<ResultView<CourseDTO>> HardDeleteCourseAsync(Guid courseId);
        Task<ResultView<CourseDTO>> SoftDeleteCourseAsync(Guid courseId);
        Task<ResultDataList<CourseListDTO>> GetTopRatedCoursesAsync(int count);
        Task<ResultDataList<CourseListDTO>> GetMostEnrolledCoursesAsync(int count);
        Task<ResultDataList<CourseListDTO>> GetInstructorCoursesAsync(string instructorId);
    }
}
