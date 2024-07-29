using AutoMapper;
using E_Commerce.BLL.Interfaces;
using E_Commerce.BLL.Specifications;
using E_Commerce.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<int> GetCountAsync(ProductSpecParams productParams)
        {
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);
            var count = await _unitOfWork.Repository<Product>().CountAsync(countSpec);
            return count;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var products = await  _unitOfWork.Repository<Product>().GelAllWithSpecAsync(spec);
            return products;

        }

        public async Task<IReadOnlyList<ProductType>> GetTypesAsync()
        {
            var Types = await _unitOfWork.Repository<ProductType>().GelAllAsync();
            return Types;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GelAllAsync();
            return brands;
        }
    }
}
