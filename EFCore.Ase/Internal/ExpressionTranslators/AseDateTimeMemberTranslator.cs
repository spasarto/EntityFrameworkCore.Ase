﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class AseDateTimeMemberTranslator : IMemberTranslator
    {
        private static readonly Dictionary<string, string> _datePartMapping
            = new Dictionary<string, string>
            {
                { nameof(DateTime.Year), "year" },
                { nameof(DateTime.Month), "month" },
                { nameof(DateTime.DayOfYear), "dayofyear" },
                { nameof(DateTime.Day), "day" },
                { nameof(DateTime.Hour), "hour" },
                { nameof(DateTime.Minute), "minute" },
                { nameof(DateTime.Second), "second" },
                { nameof(DateTime.Millisecond), "millisecond" }
            };

        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public AseDateTimeMemberTranslator(ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual SqlExpression? Translate(
            SqlExpression? instance,
            MemberInfo member,
            Type returnType,
            IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            var declaringType = member.DeclaringType;

            if (declaringType == typeof(DateTime)
                || declaringType == typeof(DateTimeOffset))
            {
                var memberName = member.Name;

                if (_datePartMapping.TryGetValue(memberName, out var datePart))
                {
                    return _sqlExpressionFactory.Function(
                        "DATEPART",
                        new[] { _sqlExpressionFactory.Fragment(datePart), instance },
                        returnType);
                }

                switch (memberName)
                {
                    case nameof(DateTime.Date):
                        return _sqlExpressionFactory.Function(
                            "CONVERT",
                            new[] { _sqlExpressionFactory.Fragment("date"), instance },
                            returnType,
                            instance.TypeMapping);

                    case nameof(DateTime.TimeOfDay):
                        return _sqlExpressionFactory.Convert(instance, returnType);

                    case nameof(DateTime.Now):
                        return _sqlExpressionFactory.Function(
                            declaringType == typeof(DateTime) ? "GETDATE" : "SYSDATETIMEOFFSET",
                            Array.Empty<SqlExpression>(),
                            returnType);

                    case nameof(DateTime.UtcNow):
                        var serverTranslation = _sqlExpressionFactory.Function(
                            declaringType == typeof(DateTime) ? "GETUTCDATE" : "SYSUTCDATETIME",
                            Array.Empty<SqlExpression>(),
                            returnType);

                        return declaringType == typeof(DateTime)
                            ? (SqlExpression)serverTranslation
                            : _sqlExpressionFactory.Convert(serverTranslation, returnType);

                    case nameof(DateTime.Today):
                        return _sqlExpressionFactory.Function(
                            "CONVERT",
                            new SqlExpression[]
                            {
                                _sqlExpressionFactory.Fragment("date"),
                                _sqlExpressionFactory.Function(
                                    "GETDATE",
                                    Array.Empty<SqlExpression>(),
                                    typeof(DateTime))
                            },
                            returnType);
                }
            }

            return null;
        }
    }
}
