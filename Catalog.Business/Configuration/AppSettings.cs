using Catalog.Business.Configuration.Settings;
using Microsoft.Extensions.Configuration;

namespace Catalog.Business.Configuration
{
    public class AppSettings
    {
        private readonly IConfiguration _configuration;

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public RabbitMqServerSettings RabbitMqServerSettings
        {
            get
            {
                var settings = new RabbitMqServerSettings();
                var section = _configuration.GetSection(Constants.RabbitMqServerSettings)
                    .GetChildren();

                settings.ConnectionString = section.SingleOrDefault(x => x.Key == Constants.RabbitMqServerHost)?.Value;
                settings.Queue = section.SingleOrDefault(x => x.Key == Constants.RabbitMqServerQueue)?.Value;

                return settings;
            }
        }
    }
}
