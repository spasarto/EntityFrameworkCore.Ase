using EntityFrameworkCore.Ase.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.Ase
{
    public static class AseDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder UseAse(this DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            var extension = (AseOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            return optionsBuilder;
        }

        private static AseOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.Options.FindExtension<AseOptionsExtension>()
               ?? new AseOptionsExtension();

    }
}
