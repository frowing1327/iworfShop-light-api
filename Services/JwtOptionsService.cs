using System.Text;
using System.Text.Json;
using iworfShop_backend_light.Data;
using iworfShop_backend_light.Models.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace iworfShop_backend_light.Services;

public class JwtOptionsService
{
    private readonly IRedisClient _redis;

    // public JwtOptionsService(IRedisClient redis)
    // {
    //     _redis = redis;
    // }
    
    public JwtOptionsService() { }

    public async Task<TokenValidationParameters> GetTokenValidationParametersAsync()
    {
        var key = await _redis.GetValueAsync("JwtConfig-Key");
        var issuer = await _redis.GetValueAsync("JwtConfig-Issuer");
        var audience = await _redis.GetValueAsync("JwtConfig-Audience");

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
        {
            await _redis.SetValueAsync("JwtConfig-Key", Guid.NewGuid().ToString());
            await _redis.SetValueAsync("JwtConfig-Issuer", "iworfShopAPI");
            await _redis.SetValueAsync("JwtConfig-Audience", "iworfShopMobile");

            // TEKRAR OKU
            key = await _redis.GetValueAsync("JwtConfig-Key");
            issuer = await _redis.GetValueAsync("JwtConfig-Issuer");
            audience = await _redis.GetValueAsync("JwtConfig-Audience");

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new Exception("redis jwt kısmı cortladı");
            }
        }

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    }
    public async Task<TokenValidationParameters> GetTokenValidationFromStaticFileAsync()
    {
        JwtConfigModel config = new JwtConfigModel();
        try
        {
            var configFile = await File.ReadAllTextAsync("jwtConfig.json");
            config = JsonSerializer.Deserialize<JwtConfigModel>(configFile);

            // TODO: validasyon mevzusu öğrenilecek
            if (string.IsNullOrEmpty(config?.MobileClient.Key) ||
                string.IsNullOrEmpty(config?.MobileClient.Issuer) || 
                string.IsNullOrEmpty(config?.MobileClient.Audience))
            {
                throw new Exception("static jwt kısmı cortladı");
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
        
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config.MobileClient.Issuer,
            ValidAudience = config.MobileClient.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.MobileClient.Key))
        };
    }
    public async Task<string> GetJwtValueAsync(string key)
    {
        JwtConfigModel config = new JwtConfigModel();
        try
        {
            var configFile = await File.ReadAllTextAsync("jwtConfig.json");
            config = JsonSerializer.Deserialize<JwtConfigModel>(configFile);

            // TODO: validasyon mevzusu öğrenilecek
            if (string.IsNullOrEmpty(config?.MobileClient.Key) ||
                string.IsNullOrEmpty(config?.MobileClient.Issuer) || 
                string.IsNullOrEmpty(config?.MobileClient.Audience))
            {
                throw new Exception("static jwt kısmı cortladı");
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }

        switch (key)
        {
            case "key":
                return config.MobileClient.Key;
            case "issuer":
                return config.MobileClient.Issuer;
            case "audience":
                return config.MobileClient.Audience;
            default:
                return "";
        }
    }
}
