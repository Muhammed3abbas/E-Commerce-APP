﻿using E_Commerce.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.BLL.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification :BaseSpecification<Product>
    {

        public ProductsWithTypesAndBrandsSpecification()
        {
            
        }

        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            : base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId.Value) &&
            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId.Value)
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1),
                productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) :base(p=>p.Id ==id) 
        {

            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }


    }
}
