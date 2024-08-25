using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context = context;
        private readonly Dictionary<Type, object> _repositories = [];

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IGenericRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new GenericRepository<TEntity>(_context);
            _repositories[typeof(TEntity)] = repository;
            return repository;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}