using E_Commerce.BLL.Specifications;
using E_Commerce.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.BLL.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams productParams);
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetTypesAsync();
        Task<Product?> GetProductAsync(int id);
        Task<int> GetCountAsync(ProductSpecParams productSpecParams);

    }
}
