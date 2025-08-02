using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity :  BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool TrackChanges,
    params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
    
}
