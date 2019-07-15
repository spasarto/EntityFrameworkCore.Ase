
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class AseCompositeMethodCallTranslator : RelationalCompositeMethodCallTranslator
    {
        private static readonly IMethodCallTranslator[] _methodCallTranslators =
        {
            new AseContainsOptimizedTranslator(),
            new AseConvertTranslator(),
            new AseDateAddTranslator(),
            new AseDateDiffTranslator(),
            new AseEndsWithOptimizedTranslator(),
            new AseFullTextSearchMethodCallTranslator(),
            new AseMathTranslator(),
            new AseNewGuidTranslator(),
            new AseObjectToStringTranslator(),
            new AseStartsWithOptimizedTranslator(),
            new AseStringIsNullOrWhiteSpaceTranslator(),
            new AseStringReplaceTranslator(),
            new AseStringSubstringTranslator(),
            new AseStringToLowerTranslator(),
            new AseStringToUpperTranslator(),
            new AseStringTrimEndTranslator(),
            new AseStringTrimStartTranslator(),
            new AseStringTrimTranslator(),
            new AseStringIndexOfTranslator(),
            new AseStringConcatMethodCallTranslator()
        };

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public AseCompositeMethodCallTranslator(
            RelationalCompositeMethodCallTranslatorDependencies dependencies)
            : base(dependencies)
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            AddTranslators(_methodCallTranslators);
        }
    }
}
