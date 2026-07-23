using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithIdSpecifications :BaseSpecification<Product,int>
    {
        public ProductWithIdSpecifications(HashSet<int> productIds):base(p=>productIds.Contains(p.Id))
        {
            
        }
    }
}
