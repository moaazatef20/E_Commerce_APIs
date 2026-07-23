using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class ProductServices(IUnitOfWork unitOfWork , IMapper mapper) : IProductService
    {
        public async Task<Result<IReadOnlyList<BrandDTO>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var brandes = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAysnc(ct);
            var data = mapper.Map<IReadOnlyList<BrandDTO>>(brandes);
            return Result<IReadOnlyList<BrandDTO>>.OK(data);
        }
        public async Task<Result<IReadOnlyList<TypeDTO>>> GetAllTypesAsync(CancellationToken ct = default)
        {
            var Types = await unitOfWork.GetRepository<ProductType, int>().GetAllAysnc(ct);
            var data = mapper.Map<IReadOnlyList<TypeDTO>>(Types);
            return Result<IReadOnlyList<TypeDTO>>.OK(data);
        }

        public async Task<Result<PaginatedResult<ProductDTO>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default)
        {
            var spec = new ProductWithBrandAndTypeSpec(queryParams);
            var productes = await unitOfWork.GetRepository<Product, int>().GetAllAysnc(spec,ct);
            var data = mapper.Map<IReadOnlyList<ProductDTO>>(productes);
            var count = await unitOfWork.GetRepository<Product, int>().GetCountAsync(new ProductCountSpecifications(queryParams));
            var result = new PaginatedResult<ProductDTO>(queryParams.PageIndex,queryParams.PageSize, count, data);
            return Result< PaginatedResult<ProductDTO>>.OK(result);
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
            var spec = new ProductWithBrandAndTypeSpec(id); //6
            var product = await unitOfWork.GetRepository<Product,int>().GetByIdAysnc(spec, ct);
            if(product == null)
                return Error.NotFound($"Product With Id {id} Not Found");
            return mapper.Map<ProductDTO>(product);
        }
    }
}
