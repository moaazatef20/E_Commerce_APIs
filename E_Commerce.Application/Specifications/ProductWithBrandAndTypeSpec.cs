using E_Commerce.Application.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithBrandAndTypeSpec :BaseSpecification<Product,int>
    {
        public ProductWithBrandAndTypeSpec(ProductQueryParams queryParams)
            : base(p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId.Value)
                     && (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId.Value)
                     && (string.IsNullOrWhiteSpace(queryParams.ShearchValue) || p.Name.ToLower().Contains(queryParams.ShearchValue.ToLower())))
        {
            AddInClude(p => p.ProductBrand);
            AddInClude(p => p.ProductType);

            switch(queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p=> p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(p=> p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public ProductWithBrandAndTypeSpec(int id):base( x=> x.Id == id) //5
        {
            AddInClude(p => p.ProductBrand);
            AddInClude(p => p.ProductType);
        }
    }
}
