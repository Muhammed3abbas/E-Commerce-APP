using E_Commerce.BLL.Specifications;
using E_Commerce.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        //Task<T> CreateAsync(T entity);
        //Task<T> UpdateAsync(T entity);
        //Task<T> DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GelAllAsync();
        Task<IReadOnlyList<T>> GelAllWithSpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
