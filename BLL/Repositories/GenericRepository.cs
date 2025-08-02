using BLL.Interfaces;
using DAL.Data.Contexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly OrderManagmentDbContext _dbcontext;

        public GenericRepository(OrderManagmentDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool TrackChanges,
    params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbcontext.Set<TEntity>();

            if (!TrackChanges)
                query = query.AsNoTracking();

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbcontext.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbcontext.Set<TEntity>().AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _dbcontext.Set<TEntity>().Update(entity);   
        }
        

        public void Delete(TEntity entity)
        {
            _dbcontext.Set<TEntity>().Remove(entity);
        }

        public async Task<TEntity?> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbcontext.Set<TEntity>();

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(predicate);
        }
    }
}
