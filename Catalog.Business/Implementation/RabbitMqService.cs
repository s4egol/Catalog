using Catalog.Business.Configuration;
using Catalog.Business.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Catalog.Business.Implementation
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly AppSettings _appSettings;

        public RabbitMqService(AppSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public void SendMessage<T>(string queue, T obj)
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));

            var factory = new ConnectionFactory { HostName = _appSettings.RabbitMqServerSettings.ConnectionString };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
      
            channel.QueueDeclare(queue: queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

            channel.BasicPublish(exchange: string.Empty,
                routingKey: queue,
                basicProperties: null,
                body: body);            
        }
    }
}
