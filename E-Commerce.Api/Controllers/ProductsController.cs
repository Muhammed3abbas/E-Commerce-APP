using AutoMapper;
using E_Commerce.Api.Dtos;
using E_Commerce.Api.Errors;
using E_Commerce.Api.Helpers;
using E_Commerce.BLL.Interfaces;
using E_Commerce.BLL.Specifications;
using E_Commerce.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{

    public class ProductsController : BaseApiController
        
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productsBrands;
        private readonly IGenericRepository<ProductType> _productsTypes;
        public ProductsController(IGenericRepository<Product> producstRepo, IMapper mapper, IGenericRepository<ProductBrand> productsBrands, IGenericRepository<ProductType> productsTypes)
        {

            _productsRepo = producstRepo;
            _mapper = mapper;
            _productsBrands = productsBrands;
            _productsTypes = productsTypes;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {

            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var products = await _productsRepo.GelAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);
            var count = await _productsRepo.CountAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize,count,data));

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpecAsync(spec);
            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product,ProductToReturnDto>(product);

        }



        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>>GetBrands ()
        {
            var brands = await _productsBrands.GelAllAsync();
            return Ok(brands);

        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>>GetTypes ()
        {
            var Types = await _productsTypes.GelAllAsync();
            return Ok(Types);

        }
    }
}
