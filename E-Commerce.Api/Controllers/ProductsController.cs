using AutoMapper;
using E_Commerce.Api.Dtos;
using E_Commerce.Api.Errors;
using E_Commerce.Api.Helpers;
using E_Commerce.BLL.Interfaces;
using E_Commerce.BLL.Specifications;
using E_Commerce.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{

    public class ProductsController : BaseApiController
        
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        //private readonly IGenericRepository<Product> _productsRepo;
        //private readonly IGenericRepository<ProductBrand> _productsBrands;
        //private readonly IGenericRepository<ProductType> _productsTypes;
        public ProductsController(IGenericRepository<Product> producstRepo, IMapper mapper, IGenericRepository<ProductBrand> productsBrands, IGenericRepository<ProductType> productsTypes, IProductService productService)
        {

            //_productsRepo = producstRepo;
            _mapper = mapper;
            //_productsBrands = productsBrands;
            //_productsTypes = productsTypes;
            _productService = productService;
        }
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {

            //var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var products = await _productService.GetProductsAsync(productParams);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            //var countSpec = new ProductsWithFiltersForCountSpecification(productParams);
            var count = await _productService.GetCountAsync(productParams);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize,count,data));

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            //var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productService.GetProductAsync(id);
            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product,ProductToReturnDto>(product);

        }



        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>>GetBrands ()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(brands);

        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>>GetTypes ()
        {
            var Types = await _productService.GetTypesAsync();
            return Ok(Types);

        }
    }
}
