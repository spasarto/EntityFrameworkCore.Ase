using Microsoft.Extensions.Configuration;

namespace EntityFrameworkCore.Ase.Tests
{
    internal class TestConfigurationBuilderFactory
    {
        public IConfigurationBuilder Create()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            configurationBuilder.AddUserSecrets("79a3edd0-2092-40ff-a04d-dcb46d5cafff");

            return configurationBuilder;
        }
    }
}
