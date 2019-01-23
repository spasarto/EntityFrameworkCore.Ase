using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace EntityFrameworkCore.Ase.Internal
{
    internal class AseCompositeMethodCallTranslator : RelationalCompositeMethodCallTranslator
    {
        public AseCompositeMethodCallTranslator(RelationalCompositeMethodCallTranslatorDependencies dependencies) 
            : base(dependencies)
        {
        }
    }
}
