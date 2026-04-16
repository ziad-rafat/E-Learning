using E_Learning.Application.Contract;
using E_Learning.Context;
using E_Learning.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Infrastructure
{
    public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(ELearningContext context) : base(context)
        {
        }

        public async Task<Category> SearchByName(string name)
        {
            return await _entities.Where(c => c.Name == name).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(Guid categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }

       
    }
}
