using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EntityFrameworkCore.Ase.Internal
{
    public class AseOptionsExtension : RelationalOptionsExtension
    {
        public override DbContextOptionsExtensionInfo Info => new ExtensionInfo(this);

        public override void ApplyServices(IServiceCollection services)
        {
            services.AddEntityFrameworkAse();
        }

        protected override RelationalOptionsExtension Clone()
        {
            return new AseOptionsExtension();
        }
        
        private sealed class ExtensionInfo : RelationalExtensionInfo
        {
            public ExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension)
            {
            }

            public override bool IsDatabaseProvider => true;

            public override string LogFragment => string.Empty;

            public override long GetServiceProviderHashCode() => 1;

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) { }
        }
    }
}
