using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using iworfShop_backend_light.Models.Entities;
using Microsoft.Extensions.Azure;

namespace iworfShop_backend_light.Services;

public interface IServiceBus
{ 
    void ProductAdded(Product product);
}

public class ServiceBus(IAzureClientFactory<ServiceBusClient> serviceBusClientFactory) : IServiceBus
{
    public void ProductAdded(Product product)
    {
        var client = serviceBusClientFactory.CreateClient("servicebus_client");
        var sender = client.CreateSender("product_added");
        
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(product)));
        sender.SendMessageAsync(message);
    }
}