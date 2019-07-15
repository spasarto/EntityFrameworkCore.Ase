using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Utilities;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class AseFullTextSearchMethodCallTranslator : IMethodCallTranslator
    {
        private const string FreeTextFunctionName = "FREETEXT";
        private const string ContainsFunctionName = "CONTAINS";

        private static readonly MethodInfo _freeTextMethodInfo
            = typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                nameof(AseDbFunctionsExtensions.FreeText),
                new[] { typeof(DbFunctions), typeof(string), typeof(string) });

        private static readonly MethodInfo _freeTextMethodInfoWithLanguage
            = typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                nameof(AseDbFunctionsExtensions.FreeText),
                new[] { typeof(DbFunctions), typeof(string), typeof(string), typeof(int) });

        private static readonly MethodInfo _containsMethodInfo
            = typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                nameof(AseDbFunctionsExtensions.Contains),
                new[] { typeof(DbFunctions), typeof(string), typeof(string) });

        private static readonly MethodInfo _containsMethodInfoWithLanguage
            = typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                nameof(AseDbFunctionsExtensions.Contains),
                new[] { typeof(DbFunctions), typeof(string), typeof(string), typeof(int) });

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            if (Equals(methodCallExpression.Method, _freeTextMethodInfo))
            {
                ValidatePropertyReference(methodCallExpression.Arguments[1]);

                return new SqlFunctionExpression(
                    FreeTextFunctionName,
                    typeof(bool),
                    new[]
                    {
                        methodCallExpression.Arguments[1],
                        methodCallExpression.Arguments[2]
                    });
            }

            if (Equals(methodCallExpression.Method, _freeTextMethodInfoWithLanguage))
            {
                ValidatePropertyReference(methodCallExpression.Arguments[1]);

                return new SqlFunctionExpression(
                    FreeTextFunctionName,
                    typeof(bool),
                    new[]
                    {
                        methodCallExpression.Arguments[1],
                        methodCallExpression.Arguments[2],
                        new SqlFragmentExpression(
                            $"LANGUAGE {((ConstantExpression)methodCallExpression.Arguments[3]).Value}")
                    });
            }

            if (Equals(methodCallExpression.Method, _containsMethodInfo))
            {
                ValidatePropertyReference(methodCallExpression.Arguments[1]);

                return new SqlFunctionExpression(
                    ContainsFunctionName,
                    typeof(bool),
                    new[]
                    {
                        methodCallExpression.Arguments[1],
                        methodCallExpression.Arguments[2]
                    });
            }

            if (Equals(methodCallExpression.Method, _containsMethodInfoWithLanguage))
            {
                ValidatePropertyReference(methodCallExpression.Arguments[1]);

                return new SqlFunctionExpression(
                    ContainsFunctionName,
                    typeof(bool),
                    new[]
                    {
                        methodCallExpression.Arguments[1],
                        methodCallExpression.Arguments[2],
                        new SqlFragmentExpression(
                            $"LANGUAGE {((ConstantExpression)methodCallExpression.Arguments[3]).Value}")
                    });
            }

            return null;
        }

        private static void ValidatePropertyReference(Expression expression)
        {
            expression = expression.RemoveConvert();
            if (expression is NullableExpression nullableExpression)
            {
                expression = nullableExpression.Operand;
            }

            if (!(expression is ColumnExpression))
            {
                throw new InvalidOperationException("The expression passed to the 'propertyReference' parameter of the 'FreeText' method is not a valid reference to a property. The expression should represent a reference to a full-text indexed property on the object referenced in the from clause: 'from e in context.Entities where EF.Functions.FreeText(e.SomeProperty, textToSearchFor) select e'");
            }
        }
    }
}
