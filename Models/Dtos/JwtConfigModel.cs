namespace iworfShop_backend_light.Models.Dtos;

public class JwtConfigModel
{
    public JwtConfigClient MobileClient  { get; set; }
    public JwtConfigClient TestClient  { get; set; }
}

public class JwtConfigClient
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}