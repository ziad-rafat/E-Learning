using E_Learning.Application.IService;
using E_Learning.Dtos.Course;
using E_Learning.Dtos.ViewResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace E_Learning.View.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(
            ICourseService courseService,
            ILogger<CourseController> logger)
        {
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        /// <response code="201">Course created successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized</response>
        [HttpPost]
        //[Authorize(Roles = "Instructor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateCourse([FromForm] CreateOrUpdateCourseDTO courseDto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _logger.LogInformation("Creating new course with title: {Title}", courseDto.Title);

            try
            {
                var result = await _courseService.CreateCourseAsync(courseDto);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to create course: {Message}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Successfully created course with ID: {CourseId}", result.Entity.Id);
                return CreatedAtAction(
                    nameof(GetCourse),
                    new { courseId = result.Entity.Id },
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course with title: {Title}", courseDto.Title);
                return StatusCode(500, new ResultView<CreateOrUpdateCourseDTO>
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the course"
                });
            }
        }


        [HttpPut]
        //[Authorize(Roles = "Instructor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCourse([FromForm] CreateOrUpdateCourseDTO courseDto)
        {
            try
            {
                var result = await _courseService.UpdateCourseAsync(courseDto);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to update course {CourseId}: {Message}", courseDto.Id, result.Message);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully updated course {CourseId}", courseDto.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course {CourseId}", courseDto.Id);
                return StatusCode(500, new ResultView<CreateOrUpdateCourseDTO>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the course"
                });
            }
        }


        [HttpGet("{courseId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourse(Guid courseId)
        {
            try
            {
                var result = await _courseService.GetCourseAsync(courseId);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Course not found: {CourseId}", courseId);
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving course: {CourseId}", courseId);
                return StatusCode(500, new ResultView<CourseDTO>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the course"
                });
            }
        }

        [HttpGet("all")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                _logger.LogInformation("Retrieving all courses");
                var result = await _courseService.GetAllCoursesAsync();
                _logger.LogInformation("Retrieved {Count} courses", result.Count);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all courses");
                return StatusCode(500, new ResultDataList<CourseListDTO>());
            }
        }


        [HttpGet("search")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses([FromQuery] CourseSearchDTO parameters)
        {
            try
            {
                _logger.LogInformation("Retrieving courses with search parameters: {@Parameters}", parameters);
                var result = await _courseService.GetAllCoursesAsync(parameters);
                _logger.LogInformation("Retrieved {Count} courses", result.Count);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving courses with parameters: {@Parameters}", parameters);
                return StatusCode(500, new ResultDataList<CourseListDTO>());
            }
        }

        
        /// <param name="count">Number of courses to retrieve</param>
        [HttpGet("top-rated")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopRatedCourses([FromQuery] int count = 10)
        {
            try
            {
                var result = await _courseService.GetTopRatedCoursesAsync(count);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top rated courses");
                return StatusCode(500, new ResultDataList<CourseListDTO>());
            }
        }

        
        [HttpGet("most-enrolled")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMostEnrolledCourses([FromQuery] int count = 10)
        {
            try
            {
                var result = await _courseService.GetMostEnrolledCoursesAsync(count);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving most enrolled courses");
                return StatusCode(500, new ResultDataList<CourseListDTO>());
            }
        }

        
        [HttpGet("by-instructor/{instructorId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInstructorCourses(string instructorId)
        {
            try
            {
                var result = await _courseService.GetInstructorCoursesAsync(instructorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving instructor courses: {InstructorId}", instructorId);
                return StatusCode(500, new ResultDataList<CourseListDTO>());
            }
        }

        
        [HttpDelete("{courseId}")]
        //[Authorize(Roles = "Instructor,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            try
            {
                var result = await _courseService.SoftDeleteCourseAsync(courseId);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to delete course {CourseId}: {Message}", courseId, result.Message);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully soft deleted course {CourseId}", courseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting course {CourseId}", courseId);
                return StatusCode(500, new ResultView<CourseDTO>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the course"
                });
            }
        }

        
        [HttpDelete("{courseId}/hard")]
        //[Authorize(Policy = "InstructorPolicy")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> HardDeleteCourse(Guid courseId)
        {
            try
            {
                _logger.LogWarning("Attempting permanent deletion of course {CourseId}", courseId);
                var result = await _courseService.HardDeleteCourseAsync(courseId);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to delete course {CourseId}: {Message}", courseId, result.Message);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully deleted course {CourseId}", courseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error permanently deleting course {CourseId}", courseId);
                return StatusCode(500, new ResultView<CourseDTO>
                {
                    IsSuccess = false,
                    Message = "An error occurred while permanently deleting the course"
                });
            }
        }
    }
}