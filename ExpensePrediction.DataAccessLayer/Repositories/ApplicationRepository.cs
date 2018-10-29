using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.Shared;
using Microsoft.EntityFrameworkCore;

namespace ExpensePrediction.DataAccessLayer.Repositories
{
    public class ApplicationRepository<TEntity> : IApplicationRepository<TEntity> where TEntity : class, IEntity
    {
        protected ApplicationDbContext _dbContext;

        public ApplicationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual async Task CreateAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindByConditionAync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().Where(expression).ToListAsync();
        }

        public virtual async Task<TEntity> FindByIdAsync(string id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual Task<int> SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public virtual void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public virtual async Task<bool> HasKey(string key)
        {
            return await _dbContext.Set<TEntity>().CountAsync(e => e.Id == key) > 0;
        }
        
    }
}