using BLL.Interfaces;
using DAL.Data.Contexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly OrderManagmentDbContext _dbcontext;
        private OrderManagmentDbContext dbContext;

        public GenericRepository(OrderManagmentDbContext dbContext, OrderManagmentDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public GenericRepository(OrderManagmentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool TrackChanges)
        {
            if (TrackChanges) return await _dbcontext.Set<TEntity>().ToListAsync();
            else return await _dbcontext.Set<TEntity>().AsNoTracking().ToListAsync();

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

        
    }
}
