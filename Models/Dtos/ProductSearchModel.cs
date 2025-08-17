using iworfShop_backend_light.Models.Entities;

namespace iworfShop_backend_light.Models.Dtos;

public class ProductSearchModel
{
    public int BrandId { get; set; }
    public List<string> Skus { get; set; }
    public List<string> BaseSkus { get; set; }
    public string Gender { get; set; }
    public bool IsActive { get; set; }
}

public class ProductSearchResultModel
{
    public List<Product> Products { get; set; } 
}