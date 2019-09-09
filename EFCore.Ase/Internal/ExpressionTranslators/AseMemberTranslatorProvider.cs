using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    class AseMemberTranslatorProvider : RelationalMemberTranslatorProvider
    {
        public AseMemberTranslatorProvider(RelationalMemberTranslatorProviderDependencies dependencies)
            : base(dependencies)
        {
            var sqlExpressionFactory = dependencies.SqlExpressionFactory;

            AddTranslators(
                new IMemberTranslator[]
                {
                    new AseDateTimeMemberTranslator(sqlExpressionFactory),
                    new AseStringMemberTranslator(sqlExpressionFactory)
                });
        }
    }
}
