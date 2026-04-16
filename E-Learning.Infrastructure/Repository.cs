using E_Learning.Application.Contract;
using E_Learning.Context;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Infrastructure
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
    {
        protected readonly ELearningContext _context;
        protected readonly DbSet<TEntity> _entities;
        public Repository(ELearningContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            return (await _entities.AddAsync(entity)).Entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(_entities.Update(entity).Entity);
        }

        public Task<TEntity> DeleteAsync(TEntity entity)
        {
            return Task.FromResult(_entities.Remove(entity).Entity);
        }

        public Task<IQueryable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(_entities.Select(e => e));
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
