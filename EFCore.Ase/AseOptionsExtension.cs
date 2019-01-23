using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Ase
{
    internal class AseOptionsExtension : RelationalOptionsExtension
    {
        public AseOptionsExtension()
        {

        }

        protected AseOptionsExtension(AseOptionsExtension copyFrom)
            : base(copyFrom)
        {
        }

        public override bool ApplyServices(IServiceCollection services)
        {
            services.AddEntityFrameworkAse();
            return true;
        }

        protected override RelationalOptionsExtension Clone()
        {
            return new AseOptionsExtension(this);
        }
    }
}
