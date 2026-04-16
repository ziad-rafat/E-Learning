using E_Learning.Application.IService;
using E_Learning.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.View.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO account, string? RoleName = "user")
        {
            try
            {
                var result = await _userService.Registration(account, RoleName);
                if (result.IsSuccess)
                {
                    return Ok(new { message = "Registration successful. Please confirm your email." });
                }
                else
                {
                    return Problem(statusCode: 400, title: "Registration Failed", detail: result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration");
                return StatusCode(500, "An internal server error occurred");
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userDto)
        {
            try
            {
                var result = await _userService.LoginAsync(userDto);

                if (result.IsSuccess)
                {
                    return Ok(result.Entity);
                }
                else
                {
                    return Problem(statusCode: 400, title: "Invalid credentials", detail: result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login");
                return StatusCode(500, "An internal server error occurred");
            }
        }


        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _userService.LogoutUser();
                return Ok(new { Message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging out");
                return StatusCode(500, "An internal server error occurred");
            }
        }


        [HttpGet("GetAllUsers")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                if (users.IsSuccess)
                {
                    return Ok(users);
                }
                else
                {
                    return Problem(statusCode: 400, title: "Failed to get all users", detail: users.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all users");
                return StatusCode(500, "An internal server error occurred");
            }
        }


        [HttpGet("AllUsersPaging/{items}/{pageNumber}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> AllUsersPaging(int items, int pageNumber = 1)
        {
            try
            {
                var users = await _userService.GetAllUsersPages(items, pageNumber);
                if (users.IsSuccess)
                {
                    return Ok(users);
                }
                else
                {
                    return Problem(statusCode: 400, title: "Failed to get paginated users", detail: users.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated users");
                return StatusCode(500, "An internal server error occurred");
            }
        }


        [HttpPost("DeleteUser")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser([FromBody] string email)
        {
            try
            {
                var resultView = await _userService.SoftDeleteUser(email);

                if (resultView.IsSuccess)
                {
                    return Ok(resultView);
                }
                else
                {
                    return Problem(statusCode: 400, title: "Failed to delete user", detail: resultView.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user");
                return StatusCode(500, "An internal server error occurred");
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)

        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid email confirmation request.");
            }
            
            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.Succeeded)
            {
                return Ok(new { message = "Email confirmed successfully!" });
            }

            return BadRequest(result.Errors);
        }
        //[HttpGet("GetAllInstrucors")]
        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> GetAllInstrucors()
        //{
        //    try
        //    {
        //        var resultView = await _userService.GetAllInstrucors();
        //        if (resultView.Count > 0)
        //        {
        //            return Ok(resultView);
        //        }
        //        else
        //        {
        //            return Problem(statusCode: 404, title: "No instrucors found", detail: "The requested resource was not found.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while getting all instrucors");
        //        return StatusCode(500, "An internal server error occurred");
        //    }
        //}


        //[HttpPatch("BlockOrUnBlock")]
        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> BlockOrUnBlockUser([FromBody] BlockUserDTO blockUserDTO)
        //{
        //    try
        //    {
        //        var resultView = await _userService.BlockOrUnBlockUser(blockUserDTO);
        //        if (resultView.IsSuccess)
        //        {
        //            return Ok(resultView);
        //        }
        //        else
        //        {
        //            return Problem(statusCode: 400, title: "Failed to block/unblock user", detail: resultView.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while blocking/unblocking user");
        //        return StatusCode(500, "An internal server error occurred");
        //    }
        //}

    }
}
