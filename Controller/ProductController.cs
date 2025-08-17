using System.Text.Json;
using iworfShop_backend_light.Common;
using iworfShop_backend_light.Models.Dtos;
using iworfShop_backend_light.Models.Entities;
using iworfShop_backend_light.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iworfShop_backend_light.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : IworfController
    {
        private readonly IProductService  _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("deactivate")]
        public async Task<IActionResult> DeactivateProduct(int brandId, List<string> skus)
        {
            var result = await _productService.DeactivateProduct(brandId, skus);

            if (result.IsSuccess)
            {
                return Success(result.Message);
            }
            else
            {
                return Fail(result.Message);
            }
        }
        
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateProduct(int brandId, List<string> skus)
        {
            var result = await _productService.ActivateProduct(brandId, skus);

            if (result.IsSuccess)
            {
                return Success(result.Message);
            }
            else
            {
                return Fail(result.Message);
            }
        }
        
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveProduct(int brandId, List<string> skus)
        {
            var result = await _productService.DeleteProduct(brandId, skus);

            if (result.IsSuccess)
            {
                return Success(result.Message);
            }
            else
            {
                return Fail(result.Message);
            }
        }
        
        //TODO: pagination eksik
        [HttpPost("getProducts")]
        public async Task<IActionResult> GetProducts(ProductSearchModel search)
        {
            var result = await _productService.GetProducts(search);

            if (result.IsSuccess)
            {
                return Success(new ProductSearchResultModel
                {
                    Products = JsonSerializer.Deserialize<List<Product>>(result.Data)
                });
            }
            else
            {
                return Fail(result.Message);
            }
        }

        [HttpPost("importProducts")]
        public async Task<IActionResult> ImportProducts(ProductImportModel importModel)
        {
            var result = await _productService.ImportProducts(importModel);
            
            if (result.IsSuccess)
            {
                return Success(result.Message);
            }
            else
            {
                return Fail(result.Message);
            }
        }
        
    }
}
