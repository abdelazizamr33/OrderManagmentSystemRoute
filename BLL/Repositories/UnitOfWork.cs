using BLL.Interfaces;
using DAL.Data.Contexts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class UnitOfWork(OrderManagmentDbContext dbContext) : IUnitOfWork
    {
        private readonly OrderManagmentDbContext _dbContext = dbContext;
        private readonly Dictionary<string, object> _repositories = new Dictionary<string, object>();

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repo = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(type, repo);
            }
            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
