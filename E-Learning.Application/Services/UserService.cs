using AutoMapper;
using E_Learning.Application.Contract;
using E_Learning.Application.IService;
using E_Learning.Dtos.User;
using E_Learning.Dtos.ViewResult;
using E_Learning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository
            , IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _userManager = userManager;
            _mapper = mapper;
        }

        private async Task CheckOrCreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole("user"));
            }
        }

        public async Task<ResultView<RegisterDTO>> Registration(RegisterDTO account, string? RoleName = "user")
        {
            var existUserEmail = await _userManager.FindByEmailAsync(account.Email);
            var existFirstName = await _userManager.FindByNameAsync(account.FirstName);
            var existLastName = await _userManager.FindByNameAsync(account.LastName);

            if (existUserEmail != null || existFirstName != null && existLastName != null)
            {
                return new ResultView<RegisterDTO>()
                {
                    Entity = account,
                    IsSuccess = false,
                    Message = "User already exists"
                };
            }

            try
            {
                string roleNameToUse;
                if (string.IsNullOrEmpty(RoleName))
                {
                    roleNameToUse = "user";
                }
                else
                {
                    await CheckOrCreateRole(RoleName);
                    roleNameToUse = RoleName;
                }

                var newUser = new User
                {
                   // UserName = account.UserName,
                    Email = account.Email,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    UserName = account.Email.Split("@")[0],
                   // EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, account.password);

                if (!result.Succeeded)
                {
                return new ResultView<RegisterDTO>()
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = $"Failed to register user. Error: {string.Join(", ", result.Errors)}"
                };
                }

               
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = $"http://localhost:46580/api/User/confirm-email?userId={newUser.Id}&token={Uri.EscapeDataString(token)}";

                // Send the email
                await _emailService.SendEmailAsync(newUser.Email, "Confirm your email",
                    $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>");

                await _userManager.AddToRoleAsync(newUser, roleNameToUse);
                var registeredUserDto = _mapper.Map<RegisterDTO>(newUser);
                return new ResultView<RegisterDTO>()
                {
                    Entity = registeredUserDto,
                    IsSuccess = true,
                    Message = "User registered successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResultView<RegisterDTO>()
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }        

        public async Task<ResultView<UserDTO>> LoginAsync(UserLoginDTO userDto)
        {
            // if (ModelState.IsValid)
            if (userDto == null)
            {
                return new ResultView<UserDTO>
                {
                    Entity = null,
                    Message = "Login data is missing",
                    IsSuccess = false
                };
            }

            if (string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.password))
            {
                return new ResultView<UserDTO>
                {
                    Entity = null,
                    Message = "Email and password are required",
                    IsSuccess = false
                };
            }

            var oldUser = await _userManager.FindByEmailAsync(userDto.Email);
            if (oldUser == null)
            {
                return new ResultView<UserDTO>
                {
                    Entity = null,
                    Message = "Email not found",
                    IsSuccess = false
                };
            }

            if (await _userManager.IsLockedOutAsync(oldUser))
            {
                return new ResultView<UserDTO>
                {
                    Entity = null,
                    Message = "Account locked due to too many failed login attempts",
                    IsSuccess = false
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(oldUser, userDto.password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new ResultView<UserDTO>
                {
                    Entity = null,
                    Message = "Invalid password",
                    IsSuccess = false
                };
            }

            var userRoles = await _userManager.GetRolesAsync(oldUser);
            var roleName = userRoles.FirstOrDefault() ?? "No Role Assigned";

            UserDTO userObj = new UserDTO()
            {
                Email = oldUser.Email ?? "No Email",
                Id = oldUser.Id,
                Role = roleName,
                FirstName = oldUser.FirstName ?? "No First Name",
                LastName = oldUser.LastName ?? "No Last Name"
            };

            await _signInManager.SignInAsync(oldUser, userDto.rememberMe);

            return new ResultView<UserDTO>
            {
                Entity = userObj,
                Message = "Login Successful",
                IsSuccess = true
            };
        }

        public async Task<bool> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<ResultView<List<UserDTO>>> GetAllUsers()
        {
            var users = _userManager.Users.Where(u => !u.IsDeleted);

            if (users == null)
            {
                return new ResultView<List<UserDTO>>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "No users found."
                };
            }

            var userlist =await users.ToListAsync();
            var userDTOs = _mapper.Map<List<UserDTO>>(userlist);
            return new ResultView<List<UserDTO>>
            {
                Entity = userDTOs,
                IsSuccess = true,
                Message = $"Successfully retrieved all users. count = {userDTOs.Count}"
            };
        }

        public async Task<ResultView<List<UserDTO>>> GetAllUsersPages(int items, int pagenumber)
        {
            var Alldata = (_userManager.Users);
            if (Alldata == null)
            {
                return new ResultView<List<UserDTO>>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "No users found."
                };
            }
            var userlist = await Alldata.Skip(items * (pagenumber - 1)).Take(items).ToListAsync();
            var userDTOs = _mapper.Map<List<UserDTO>>(userlist);

            return new ResultView<List<UserDTO>>
            {
                Entity = userDTOs,
                IsSuccess = true,
                Message = "Successfully retrieved all users."
            };
        }

        public async Task<ResultView<UserDTO>> SoftDeleteUser(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new ResultView<UserDTO>
                    {
                        Entity = null,
                        IsSuccess = false,
                        Message = "User not found"
                    };
                }

                if (user.IsDeleted)
                {
                    return new ResultView<UserDTO>
                    {
                        Entity = _mapper.Map<UserDTO>(user),
                        IsSuccess = false,
                        Message = "User is already deleted"
                    };
                }
                user.IsDeleted = true;

                await _userRepository.SaveChangesAsync();
                //var result = await _UserManager.UpdateAsync(user);

                var userDto = _mapper.Map<UserDTO>(user);
                return new ResultView<UserDTO>
                {
                    Entity = userDto,
                    IsSuccess = true,
                    Message = "User deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResultView<UserDTO> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        //public async Task<ResultView<BlockUserDTO>> BlockOrUnBlockUser(BlockUserDTO blockUserDTO)
        //{
        //    var user = await _UserManager.FindByIdAsync(blockUserDTO.Id);

        //    if (user == null)
        //    {
        //        return new ResultView<BlockUserDTO> { Entity = null, IsSuccess = false, Message = "Unable to find the user." };
        //    }

        //    if (user.IsBlocked == blockUserDTO.IsBlocked)
        //    {
        //        return new ResultView<BlockUserDTO>
        //        {
        //            Entity = blockUserDTO,
        //            IsSuccess = false,
        //            Message = user.IsBlocked ? "The user is already blocked." : "The user is already unblocked."
        //        };
        //    }

        //    user.IsBlocked = blockUserDTO.IsBlocked;

        //    var result = await _UserManager.UpdateAsync(user);

        //    if (result.Succeeded)
        //    {
        //        return new ResultView<BlockUserDTO>
        //        {
        //            Entity = blockUserDTO,
        //            IsSuccess = true,
        //            Message = user.IsBlocked ? "User blocked successfully." : "User unblocked successfully."
        //        };
        //    }
        //    return new ResultView<BlockUserDTO> { Entity = null, IsSuccess = false, Message = "Failed to update user." };

        //}

    }
}
