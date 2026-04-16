using AutoMapper;
using E_Learning.Application.Contract;
using E_Learning.Application.IService;
using E_Learning.Dtos.Course;
using E_Learning.Dtos.ViewResult;
using E_Learning.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Learning.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<CourseService> _logger;

        public CourseService(
            ICourseRepository courseRepository,
            IMapper mapper,
            IFileService fileService,
            ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResultView<CreateOrUpdateCourseDTO>> CreateCourseAsync(CreateOrUpdateCourseDTO courseDto)
        {
            var result = new ResultView<CreateOrUpdateCourseDTO>();
            try
            {
                _logger.LogInformation("Creating new course with title: {Title}", courseDto.Title);

                if (await IsCourseExistsByTitleAsync(courseDto.Title))
                {
                    result.IsSuccess = false;
                    result.Message = "Course with this title already exists";
                    return result;
                }

                var course = _mapper.Map<Course>(courseDto);
                await HandleFileUploadsAsync(course, courseDto);

                var newCourse = await _courseRepository.CreateAsync(course);
                await _courseRepository.SaveChangesAsync();

                result.Entity = _mapper.Map<CreateOrUpdateCourseDTO>(newCourse);
                result.IsSuccess = true;
                result.Message = "Course Created Successfully";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course: {Message}", ex.Message);
                result.IsSuccess = false;
                result.Message = "Failed to create course";
                return result;
            }
        }

        public async Task<ResultView<CreateOrUpdateCourseDTO>> UpdateCourseAsync(CreateOrUpdateCourseDTO courseDto)
        {
            var result = new ResultView<CreateOrUpdateCourseDTO>();
            try
            {
                _logger.LogInformation("Updating course with ID: {CourseId}", courseDto.Id);

                //var oldCourse = await _courseRepository.GetByIdAsync(courseDto.Id);
                var oldCourse = (await _courseRepository.GetAllAsync()).AsNoTracking().FirstOrDefault(c => c.Id == courseDto.Id);

                if (oldCourse == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Course not found";
                    return result;
                }

                var course = _mapper.Map<Course>(courseDto);
                await HandleFileUpdatesAsync(course, courseDto, oldCourse);

                var updatedCourse = await _courseRepository.UpdateAsync(course);
                //oldCourse.Title = courseDto.Title; overwrite for all prop

                await _courseRepository.SaveChangesAsync();

                //result.Entity = _mapper.Map<CreateOrUpdateCourseDTO>(oldCourse);
                result.Entity = _mapper.Map<CreateOrUpdateCourseDTO>(updatedCourse);
                result.IsSuccess = true;
                result.Message = "Course Updated Successfully";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course: {Message}", ex.Message);
                result.IsSuccess = false;
                result.Message = "Failed to update course";
                return result;
            }
        }

        public async Task<ResultDataList<CourseListDTO>> GetAllCoursesAsync()
        {
            try
            {
                var courses = (await _courseRepository.GetAllAsync())
                    .Where(c => c.IsDeleted == false)
                    .Include(c => c.Instructor)
                    .Include(c => c.Topic)
                    .ToList();

                //var courseDtos = courses
                    //.Where(c => c.IsDeleted == false)
                    //.Include(c => c.Instructor)
                    //.Include(c => c.Topic)
                    //.Select(c => new CourseListDTO
                    //{
                    //    Id = c.Id,
                    //    Title = c.Title,
                    //    Ar_Title = c.Ar_Title,
                    //    Price = c.Price,
                    //    CourseImage = c.CourseImage,
                    //    RatingAverage = c.RatingAverage,
                    //    RatingCount = c.RatingCount,
                    //    EnrollmentsCount = c.EnrollmentsCount,
                    //    InstructorFirstName = c.Instructor.FirstName,
                    //    InstructorLastName = c.Instructor.FirstName,
                    //    TopicName = c.Topic.Name
                    //})
                    //.ToList();

                var courseDtos = _mapper.Map<List<CourseListDTO>>(courses);
                return new ResultDataList<CourseListDTO>
                {
                    Entities = courseDtos,
                    Count = courseDtos.Count
                };
            }
            catch (Exception ex)
            {
                return new ResultDataList<CourseListDTO>
                {
                    Entities = new List<CourseListDTO>(),
                    Count = 0
                };
            }
        }

        public async Task<ResultDataList<CourseListDTO>> GetAllCoursesAsync(CourseSearchDTO parameters)
        {
            var result = new ResultDataList<CourseListDTO>();
            try
            {
                _logger.LogInformation("Retrieving courses with search parameters");

                var query = await BuildSearchQuery(parameters);
                var totalCount = await query.CountAsync();

                var courses = await ApplyPagination(query, parameters)
                    .ToListAsync();

                result.Entities = _mapper.Map<List<CourseListDTO>>(courses);
                result.Count = totalCount;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving courses: {Message}", ex.Message);
                result.Entities = new List<CourseListDTO>();
                result.Count = 0;
                return result;
            }
        }

        public async Task<ResultView<CourseDTO>> GetCourseAsync(Guid courseId)
        {
            var result = new ResultView<CourseDTO>();
            try
            {
                _logger.LogInformation("Retrieving course with ID: {CourseId}", courseId);

                var course = await _courseRepository.GetByIdAsync(courseId);
                if (course == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Course not found";
                    return result;
                }

                result.Entity = _mapper.Map<CourseDTO>(course);
                result.IsSuccess = true;
                result.Message = "Course retrieved successfully";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving course: {Message}", ex.Message);
                result.IsSuccess = false;
                result.Message = "Failed to retrieve course";
                return result;
            }
        }

        public async Task<ResultView<CourseDTO>> HardDeleteCourseAsync(Guid courseId)
        {
            var result = new ResultView<CourseDTO>();
            try
            {
                _logger.LogInformation("Hard deleting course with ID: {CourseId}", courseId);

                var course = await _courseRepository.GetByIdAsync(courseId);
                if (course == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Course not found";
                    return result;
                }

                await DeleteCourseFilesAsync(course);
                await _courseRepository.DeleteAsync(course);
                await _courseRepository.SaveChangesAsync();

                result.Entity = _mapper.Map<CourseDTO>(course);
                result.IsSuccess = true;
                result.Message = "Course hard deleted successfully";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error hard deleting course: {Message}", ex.Message);
                result.IsSuccess = false;
                result.Message = "Failed to hard delete course";
                return result;
            }
        }

        public async Task<ResultView<CourseDTO>> SoftDeleteCourseAsync(Guid courseId)
        {
            var result = new ResultView<CourseDTO>();
            try
            {
                _logger.LogInformation("Soft deleting course with ID: {CourseId}", courseId);

                var course = await _courseRepository.GetByIdAsync(courseId);
                if (course == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Course not found";
                    return result;
                }

                course.IsDeleted = true;
                await _courseRepository.SaveChangesAsync();

                result.Entity = _mapper.Map<CourseDTO>(course);
                result.IsSuccess = true;
                result.Message = "Course soft deleted successfully";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting course: {Message}", ex.Message);
                result.IsSuccess = false;
                result.Message = "Failed to soft delete course";
                return result;
            }
        }


        public async Task<ResultDataList<CourseListDTO>> GetTopRatedCoursesAsync(int count)
        {
            try
            {
                var allCourses = await _courseRepository.GetAllAsync();

                var courses = allCourses
                    .Where(c => !c.IsDeleted)
                    .OrderByDescending(c => c.RatingAverage)
                    .Take(count)
                    .Include(c => c.Instructor)
                    .Include(c => c.Topic)
                    .ToListAsync();

                var courseDtos = _mapper.Map<List<CourseListDTO>>(courses);

                return new ResultDataList<CourseListDTO>
                {
                    Entities = courseDtos,
                    Count = courseDtos.Count
                };
            }
            catch (Exception ex)
            {
                return new ResultDataList<CourseListDTO>
                {
                    Entities = new List<CourseListDTO>(),
                    Count = 0
                };
            }
        }

        public async Task<ResultDataList<CourseListDTO>> GetMostEnrolledCoursesAsync(int count)
        {
            try
            {
                var allCourses = await _courseRepository.GetAllAsync();

                var courses = allCourses
                    .Where(c => !c.IsDeleted)
                    .OrderByDescending(c => c.EnrollmentsCount)
                    .Take(count)
                    .Include(c => c.Instructor)
                    .Include(c => c.Topic)
                    .ToListAsync();

                var courseDtos = _mapper.Map<List<CourseListDTO>>(courses);

                return new ResultDataList<CourseListDTO>
                {
                    Entities = courseDtos,
                    Count = courseDtos.Count
                };
            }
            catch (Exception ex)
            {
                return new ResultDataList<CourseListDTO>
                {
                    Entities = new List<CourseListDTO>(),
                    Count = 0
                };
            }
        }

        public async Task<ResultDataList<CourseListDTO>> GetInstructorCoursesAsync(string instructorId)
        {
            try
            {
                var courses = (await _courseRepository.GetAllAsync())
                .Where(c => !c.IsDeleted && c.UserId == instructorId)
                .Include(c => c.Instructor)
                .Include(c => c.Topic)
                .ToList();
                //.AsQueryable();
                //.ToListAsync(); error

                var courseDtos = _mapper.Map<List<CourseListDTO>>(courses);

                return new ResultDataList<CourseListDTO>
                {
                    Entities = courseDtos,
                    Count = courseDtos.Count
                };
            }
            catch (Exception ex)
            {
                return new ResultDataList<CourseListDTO>
                {
                    Entities = new List<CourseListDTO>(),
                    Count = 0
                };
            }
        }


        private async Task<bool> IsCourseExistsByTitleAsync(string title)
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Any(c => c.Title == title);
        }

        private async Task HandleFileUploadsAsync(Course course, CreateOrUpdateCourseDTO courseDto)
        {
            if (courseDto.CourseImage != null)
            {
                course.CourseImage = await _fileService.UploadFileAsync(courseDto.Image, "courses/images");
            }

            if (courseDto.PromotionalVideo != null)
            {
                course.PromotionalVideo = await _fileService.UploadFileAsync(courseDto.Video, "courses/videos");
            }
        }

        private async Task HandleFileUpdatesAsync(Course course, CreateOrUpdateCourseDTO courseDto, Course oldCourse)
        {
            if (courseDto.CourseImage != null)
            {
                await DeleteFileIfExistsAsync(oldCourse.CourseImage);
                course.CourseImage = await _fileService.UploadFileAsync(courseDto.Image, "courses/images");
            }
            else
            {
                course.CourseImage = oldCourse.CourseImage;
            }

            if (courseDto.PromotionalVideo != null)
            {
                await DeleteFileIfExistsAsync(oldCourse.PromotionalVideo);
                course.PromotionalVideo = await _fileService.UploadFileAsync(courseDto.Video, "courses/videos");
            }
            else
            {
                course.PromotionalVideo = oldCourse.PromotionalVideo;
            }
        }

        private async Task DeleteFileIfExistsAsync(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                await _fileService.DeleteFileAsync(filePath);
            }
        }

        private async Task DeleteCourseFilesAsync(Course course)
        {
            await DeleteFileIfExistsAsync(course.CourseImage);
            await DeleteFileIfExistsAsync(course.PromotionalVideo);
        }

        private async Task<IQueryable<Course>> BuildSearchQuery(CourseSearchDTO parameters)
        {
            var query = (await _courseRepository.GetAllAsync())
                .Where(c => !c.IsDeleted)
                .Include(c => c.Instructor)
                .Include(c => c.Topic)
                .AsQueryable();

            query = ApplyFilters(query, parameters);
            query = ApplySorting(query, parameters);

            return query;
        }

        private IQueryable<Course> ApplyFilters(IQueryable<Course> query, CourseSearchDTO parameters)
        {
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(c =>
                    c.Title.Contains(parameters.SearchTerm) ||
                    c.Description.Contains(parameters.SearchTerm));
            }

            if (parameters.MinPrice.HasValue)
            {
                query = query.Where(c => c.Price >= parameters.MinPrice.Value);
            }

            if (parameters.MaxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= parameters.MaxPrice.Value);
            }

            if (parameters.TopicId.HasValue)
            {
                query = query.Where(c => c.TopicId == parameters.TopicId.Value);
            }

            if (!string.IsNullOrEmpty(parameters.InstructorId))
            {
                query = query.Where(c => c.UserId == parameters.InstructorId);
            }

            if (parameters.MinRating.HasValue)
            {
                query = query.Where(c => c.RatingAverage >= parameters.MinRating.Value);
            }

            return query;
        }

        private IQueryable<Course> ApplySorting(IQueryable<Course> query, CourseSearchDTO parameters)
        {
            return parameters.SortBy?.ToLower() switch
            {
                "price" => parameters.SortDescending
                    ? query.OrderByDescending(c => c.Price)
                    : query.OrderBy(c => c.Price),
                "rating" => parameters.SortDescending
                    ? query.OrderByDescending(c => c.RatingAverage)
                    : query.OrderBy(c => c.RatingAverage),
                "enrollments" => parameters.SortDescending
                    ? query.OrderByDescending(c => c.EnrollmentsCount)
                    : query.OrderBy(c => c.EnrollmentsCount),
                "date" => parameters.SortDescending
                    ? query.OrderByDescending(c => c.CreatedAt)
                    : query.OrderBy(c => c.CreatedAt),
                _ => query.OrderByDescending(c => c.CreatedAt)
            };
        }

        private IQueryable<Course> ApplyPagination(IQueryable<Course> query, CourseSearchDTO parameters)
        {
            return query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize);
        }

    }
}
