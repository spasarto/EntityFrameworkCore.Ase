using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace EntityFrameworkCore.Ase.Internal
{
    public interface IAseOptions : ISingletonOptions
    {
    }

    class AseOptions : IAseOptions
    {

        public virtual void Initialize(IDbContextOptions options)
        {
            var aseOptions = options.FindExtension<AseOptionsExtension>() ?? new AseOptionsExtension();
        }

        public virtual void Validate(IDbContextOptions options)
        {
            var aseOptions = options.FindExtension<AseOptionsExtension>() ?? new AseOptionsExtension();
        }
    }
}
