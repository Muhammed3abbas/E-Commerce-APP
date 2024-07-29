using E_Commerce.BLL.Interfaces;
using E_Commerce.DAL.Entities;
using E_Commerce.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }



        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null) _repositories = new Hashtable();

            //var type = typeof(TEntity).Name;
            var key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_context);
                _repositories.Add(key, repository);

               
                //var repositoryType = typeof(GenericRepository<>);
                //var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                //_repositories.Add(type, repositoryInstance);
            }

            //return (IGenericRepository<TEntity>)_repositories[type];
            return (IGenericRepository<TEntity>)_repositories[key];
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
