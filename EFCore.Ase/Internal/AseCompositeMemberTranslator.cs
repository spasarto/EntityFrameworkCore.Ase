using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace EntityFrameworkCore.Ase.Internal
{
    internal class AseCompositeMemberTranslator : RelationalCompositeMemberTranslator
    {
        public AseCompositeMemberTranslator(RelationalCompositeMemberTranslatorDependencies dependencies)
            : base(dependencies)
        {
        }
    }
}
