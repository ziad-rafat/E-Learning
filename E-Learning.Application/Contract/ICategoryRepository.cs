using E_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.Contract
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task<Category> SearchByName(string name);
        Task<bool> ExistsAsync(Guid categoryId);
    }
}
