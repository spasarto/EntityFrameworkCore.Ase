﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    public class AseSqlTranslatingExpressionVisitor : RelationalSqlTranslatingExpressionVisitor
    {
        private static readonly HashSet<string> _dateTimeDataTypes
            = new HashSet<string>
            {
                "time",
                "date",
                "datetime",
                "datetime2",
                "datetimeoffset"
            };

        private static readonly HashSet<ExpressionType> _arithmeticOperatorTypes
            = new HashSet<ExpressionType>
            {
                ExpressionType.Add,
                ExpressionType.Subtract,
                ExpressionType.Multiply,
                ExpressionType.Divide,
                ExpressionType.Modulo
            };

        // TODO: Possibly make this protected in base
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public AseSqlTranslatingExpressionVisitor(
            RelationalSqlTranslatingExpressionVisitorDependencies dependencies,
            QueryCompilationContext queryCompilationContext,
            QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
            : base(dependencies, queryCompilationContext, queryableMethodTranslatingExpressionVisitor)
        {
            _sqlExpressionFactory = dependencies.SqlExpressionFactory;
        }

        protected override Expression VisitBinary(BinaryExpression binaryExpression)
        {
            var visitedExpression = (SqlExpression)base.VisitBinary(binaryExpression);

            if (visitedExpression == null)
            {
                return null;
            }

            return visitedExpression is SqlBinaryExpression sqlBinary
                   && _arithmeticOperatorTypes.Contains(sqlBinary.OperatorType)
                   && (_dateTimeDataTypes.Contains(GetProviderType(sqlBinary.Left))
                       || _dateTimeDataTypes.Contains(GetProviderType(sqlBinary.Right)))
                ? null
                : visitedExpression;
        }

        public override SqlExpression? TranslateLongCount(SqlExpression sqlExpression)
        {
            // TODO: Translate Count with predicate for GroupBy
            return _sqlExpressionFactory.ApplyDefaultTypeMapping(
                _sqlExpressionFactory.Function("COUNT_BIG", new[] { _sqlExpressionFactory.Fragment("*") }, typeof(long)));
        }

        private static string GetProviderType(SqlExpression expression)
        {
            return expression.TypeMapping?.StoreType;
        }

        public override SqlExpression Translate(Expression expression)
        {
            return base.Translate(expression);
        }
    }
}
