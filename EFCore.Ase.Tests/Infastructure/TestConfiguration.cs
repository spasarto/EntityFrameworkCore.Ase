using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EntityFrameworkCore.Ase.Tests.Infastructure
{
    internal static class TestConfiguration
    {
        public static IConfiguration Configuration { get; set; }

        public static void Initialize()
        {
            if (Configuration != null)
                return;

            Configuration = new TestConfigurationBuilderFactory().Create().Build();
        }

        public static IOptions<TOptions> GetOptions<TOptions>(IConfiguration config = null)
            where TOptions : class, new()
        {
            TOptions options = new TOptions();
            
            (config ?? Configuration).Bind(typeof(TOptions).Name, options);

            return Options.Create(options);
        }
    }
}
