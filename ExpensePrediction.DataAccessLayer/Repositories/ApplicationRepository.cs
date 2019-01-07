using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExpensePrediction.Exceptions;
using DbUpdateException = Microsoft.EntityFrameworkCore.DbUpdateException;

namespace ExpensePrediction.DataAccessLayer.Repositories
{
    public sealed class ApplicationRepository<TEntity> : IApplicationRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression)
        {
            if(expression == null) throw new ExpressionNullException();
            return await _dbContext.Set<TEntity>().Where(expression).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> FindTopByConditionAsync(Expression<Func<TEntity, bool>> expression, int top)
        {
            return await _dbContext.Set<TEntity>().Where(expression).Take(top).ToListAsync();
        }

        public async Task<TEntity> FindByIdAsync(string id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public Task<int> SaveAsync()
        {
            try
            {
                return _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exceptions.DbUpdateException(e.Message, e);
            }
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public async Task<bool> HasKey(string key)
        {
            return await _dbContext.Set<TEntity>().CountAsync(e => e.Id == key) > 0;
        }

    }
}