using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpensePrediction.DataAccessLayer.Interfaces
{
    public interface IApplicationRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> FindAllAsync();
        Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> FindTopByConditionAsync(Expression<Func<TEntity, bool>> expression, int top);
        Task<TEntity> FindByIdAsync(string id);
        Task CreateAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> SaveAsync();
        Task<bool> HasKey(string key);
    }
}