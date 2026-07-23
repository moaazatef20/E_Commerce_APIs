using E_Commerce.API.Attributes;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [RedisCashe(1)]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery]ProductQueryParams queryParams, CancellationToken ct)
        {
            var Result = await _productService.GetAllProductsAsync(queryParams,ct);
            return toActionResult(Result);
        }

        [HttpGet("{id}")]
        
        public async Task<ActionResult<ProductDTO>> GetProduct(int id ,CancellationToken ct)
        {
            var Result = await _productService.GetProductByIdAsync(id, ct);
            return toActionResult(Result);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDTO>>> GetAllBrands(CancellationToken ct)
        {
            return toActionResult(await _productService.GetAllBrandsAsync(ct));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDTO>>> GetAllTypes(CancellationToken ct)
        {
            return toActionResult(await _productService.GetAllTypesAsync(ct));
        }
    }
}
