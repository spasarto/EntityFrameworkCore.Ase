using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Reflection;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    public class AseStringMemberTranslator : IMemberTranslator
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public AseStringMemberTranslator(ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public virtual SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType)
        {
            if (member.Name == nameof(string.Length)
                && instance?.Type == typeof(string))
            {
                return _sqlExpressionFactory.Convert(
                    _sqlExpressionFactory.Function("LEN", new[] { instance }, typeof(long)),
                    returnType);
            }

            return null;
        }
    }
}
