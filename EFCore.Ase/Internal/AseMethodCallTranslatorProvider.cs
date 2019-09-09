using EntityFrameworkCore.Ase.Internal.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.Ase.Internal
{
    class AseMethodCallTranslatorProvider : RelationalMethodCallTranslatorProvider
    {
        public AseMethodCallTranslatorProvider(RelationalMethodCallTranslatorProviderDependencies dependencies)
            : base(dependencies)
        {
            var sqlExpressionFactory = dependencies.SqlExpressionFactory;

            AddTranslators(
                new IMethodCallTranslator[]
                {
                    new AseConvertTranslator(sqlExpressionFactory),
                    new AseDateTimeMethodTranslator(sqlExpressionFactory),
                    new AseDateDiffFunctionsTranslator(sqlExpressionFactory),
                    new AseFullTextSearchFunctionsTranslator(sqlExpressionFactory),
                    new AseIsDateFunctionTranslator(sqlExpressionFactory),
                    new AseMathTranslator(sqlExpressionFactory),
                    new AseNewGuidTranslator(sqlExpressionFactory),
                    new AseObjectToStringTranslator(sqlExpressionFactory),
                    new AseStringMethodTranslator(sqlExpressionFactory)
                });
        }
    }
}
