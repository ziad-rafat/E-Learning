using E_Learning.Models;

namespace E_Learning.Application.Contract
{
    public interface IUserRepository : IRepository<User,string>
    {
        Task<User> ApproveUserAsync(int userId);

        Task<User> ChangePasswordAsync(int userId, string newPassword);

        //private string HashPassword(string password)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<User> UpdateProfileAsync(int userId, UserProfile profile);
    }
}
