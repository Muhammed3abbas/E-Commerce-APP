using E_Commerce.BLL.Interfaces;
using E_Commerce.BLL.Specifications;
using E_Commerce.DAL;
using E_Commerce.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext storeContext)
        {
            _context = storeContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }


        public async Task<IReadOnlyList<T>> GelAllAsync()
            =>await _context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GelAllWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecification(spec)
                .ToListAsync();
        

        //public async Task<T> GetAsync(int id) =>
        //    await _context.Set<T>().FindAsync(id);

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecification(spec)
                .FirstOrDefaultAsync();


        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task AddAsync(T entity)
            => _context.Set<T>().AddAsync(entity);


        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
