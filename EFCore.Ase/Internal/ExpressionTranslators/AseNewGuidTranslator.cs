using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    public class AseNewGuidTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _methodInfo = typeof(Guid).GetRuntimeMethod(nameof(Guid.NewGuid), Array.Empty<Type>());
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public AseNewGuidTranslator(ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
        {
            return _methodInfo.Equals(method)
                ? _sqlExpressionFactory.Function(
                    "NEWID",
                    Array.Empty<SqlExpression>(),
                    method.ReturnType)
                : null;
        }
    }
}
