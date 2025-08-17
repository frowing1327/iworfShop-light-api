using System.Security.Claims;
using iworfShop_backend_light.Common;
using iworfShop_backend_light.Data;
using iworfShop_backend_light.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iworfShop_backend_light.Controller;

[Route("api/[controller]")]
[ApiController]
public class TestController : IworfController
{
    //private readonly IRedisClient  _redisClient;
    private readonly IProductService _productService;
    public TestController(IProductService productService)
    {
        productService = _productService;
    }

    [Authorize]
    [HttpGet("AuthTest")]
    public async Task<IActionResult> AuthTest()
    {
        var email = User.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
        var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

        var message = $"✅ Yetkili erişim başarılı. Kullanıcı ID: {userId}, Email: {email}";
        return Success(message);
    }

    [HttpGet("importInitialTest")]
    public async Task<IActionResult> ImportInitialTest()
    {
        var result = await _productService.ImportInitialTestValues();

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
