using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    class AseDateDiffFunctionsTranslator : IMethodCallTranslator
    {
        private readonly Dictionary<MethodInfo, string> _methodInfoDateDiffMapping
            = new Dictionary<MethodInfo, string>
            {
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffYear),
                        new[] { typeof(DbFunctions), typeof(DateTime), typeof(DateTime) }),
                    "YEAR"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffYear),
                        new[] { typeof(DbFunctions), typeof(DateTime?), typeof(DateTime?) }),
                    "YEAR"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffYear),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset), typeof(DateTimeOffset) }),
                    "YEAR"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffYear),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset?), typeof(DateTimeOffset?) }),
                    "YEAR"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMonth),
                        new[] { typeof(DbFunctions), typeof(DateTime), typeof(DateTime) }),
                    "MONTH"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMonth),
                        new[] { typeof(DbFunctions), typeof(DateTime?), typeof(DateTime?) }),
                    "MONTH"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMonth),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset), typeof(DateTimeOffset) }),
                    "MONTH"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMonth),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset?), typeof(DateTimeOffset?) }),
                    "MONTH"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffDay),
                        new[] { typeof(DbFunctions), typeof(DateTime), typeof(DateTime) }),
                    "DAY"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffDay),
                        new[] { typeof(DbFunctions), typeof(DateTime?), typeof(DateTime?) }),
                    "DAY"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffDay),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset), typeof(DateTimeOffset) }),
                    "DAY"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffDay),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset?), typeof(DateTimeOffset?) }),
                    "DAY"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffHour),
                        new[] { typeof(DbFunctions), typeof(DateTime), typeof(DateTime) }),
                    "HOUR"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffHour),
                        new[] { typeof(DbFunctions), typeof(DateTime?), typeof(DateTime?) }),
                    "HOUR"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffHour),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset), typeof(DateTimeOffset) }),
                    "HOUR"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffHour),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset?), typeof(DateTimeOffset?) }),
                    "HOUR"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffHour),
                        new[] { typeof(DbFunctions), typeof(TimeSpan), typeof(TimeSpan) }),
                    "HOUR"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffHour),
                        new[] { typeof(DbFunctions), typeof(TimeSpan?), typeof(TimeSpan?) }),
                    "HOUR"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMinute),
                        new[] { typeof(DbFunctions), typeof(DateTime), typeof(DateTime) }),
                    "MINUTE"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMinute),
                        new[] { typeof(DbFunctions), typeof(DateTime?), typeof(DateTime?) }),
                    "MINUTE"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMinute),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset), typeof(DateTimeOffset) }),
                    "MINUTE"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMinute),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset?), typeof(DateTimeOffset?) }),
                    "MINUTE"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMinute),
                        new[] { typeof(DbFunctions), typeof(TimeSpan), typeof(TimeSpan) }),
                    "MINUTE"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMinute),
                        new[] { typeof(DbFunctions), typeof(TimeSpan?), typeof(TimeSpan?) }),
                    "MINUTE"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffSecond),
                        new[] { typeof(DbFunctions), typeof(DateTime), typeof(DateTime) }),
                    "SECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffSecond),
                        new[] { typeof(DbFunctions), typeof(DateTime?), typeof(DateTime?) }),
                    "SECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffSecond),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset), typeof(DateTimeOffset) }),
                    "SECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffSecond),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset?), typeof(DateTimeOffset?) }),
                    "SECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffSecond),
                        new[] { typeof(DbFunctions), typeof(TimeSpan), typeof(TimeSpan) }),
                    "SECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffSecond),
                        new[] { typeof(DbFunctions), typeof(TimeSpan?), typeof(TimeSpan?) }),
                    "SECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMillisecond),
                        new[] { typeof(DbFunctions), typeof(DateTime), typeof(DateTime) }),
                    "MILLISECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMillisecond),
                        new[] { typeof(DbFunctions), typeof(DateTime?), typeof(DateTime?) }),
                    "MILLISECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMillisecond),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset), typeof(DateTimeOffset) }),
                    "MILLISECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMillisecond),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset?), typeof(DateTimeOffset?) }),
                    "MILLISECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMillisecond),
                        new[] { typeof(DbFunctions), typeof(TimeSpan), typeof(TimeSpan) }),
                    "MILLISECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMillisecond),
                        new[] { typeof(DbFunctions), typeof(TimeSpan?), typeof(TimeSpan?) }),
                    "MILLISECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMicrosecond),
                        new[] { typeof(DbFunctions), typeof(DateTime), typeof(DateTime) }),
                    "MICROSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMicrosecond),
                        new[] { typeof(DbFunctions), typeof(DateTime?), typeof(DateTime?) }),
                    "MICROSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMicrosecond),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset), typeof(DateTimeOffset) }),
                    "MICROSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMicrosecond),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset?), typeof(DateTimeOffset?) }),
                    "MICROSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMicrosecond),
                        new[] { typeof(DbFunctions), typeof(TimeSpan), typeof(TimeSpan) }),
                    "MICROSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffMicrosecond),
                        new[] { typeof(DbFunctions), typeof(TimeSpan?), typeof(TimeSpan?) }),
                    "MICROSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffNanosecond),
                        new[] { typeof(DbFunctions), typeof(DateTime), typeof(DateTime) }),
                    "NANOSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffNanosecond),
                        new[] { typeof(DbFunctions), typeof(DateTime?), typeof(DateTime?) }),
                    "NANOSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffNanosecond),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset), typeof(DateTimeOffset) }),
                    "NANOSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffNanosecond),
                        new[] { typeof(DbFunctions), typeof(DateTimeOffset?), typeof(DateTimeOffset?) }),
                    "NANOSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffNanosecond),
                        new[] { typeof(DbFunctions), typeof(TimeSpan), typeof(TimeSpan) }),
                    "NANOSECOND"
                },
                {
                    typeof(AseDbFunctionsExtensions).GetRuntimeMethod(
                        nameof(AseDbFunctionsExtensions.DateDiffNanosecond),
                        new[] { typeof(DbFunctions), typeof(TimeSpan?), typeof(TimeSpan?) }),
                    "NANOSECOND"
                }
            };

        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public AseDateDiffFunctionsTranslator(
            ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
        {
            if (_methodInfoDateDiffMapping.TryGetValue(method, out var datePart))
            {
                var startDate = arguments[1];
                var endDate = arguments[2];
                var typeMapping = ExpressionExtensions.InferTypeMapping(startDate, endDate);

                startDate = _sqlExpressionFactory.ApplyTypeMapping(startDate, typeMapping);
                endDate = _sqlExpressionFactory.ApplyTypeMapping(endDate, typeMapping);

                return _sqlExpressionFactory.Function(
                    "DATEDIFF",
                    new[] { _sqlExpressionFactory.Fragment(datePart), startDate, endDate },
                    typeof(int));
            }

            return null;
        }
    }
}
