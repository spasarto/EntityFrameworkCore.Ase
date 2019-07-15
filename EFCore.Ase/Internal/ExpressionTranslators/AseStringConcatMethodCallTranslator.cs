using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    public class AseStringConcatMethodCallTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _stringConcatMethodInfo
            = typeof(string).GetRuntimeMethod(
                nameof(string.Concat),
                new[] { typeof(string), typeof(string) });

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Expression Translate(MethodCallExpression methodCallExpression)
            => _stringConcatMethodInfo.Equals(methodCallExpression.Method)
            ? Expression.Add(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1], _stringConcatMethodInfo)
            : null;
    }
}
