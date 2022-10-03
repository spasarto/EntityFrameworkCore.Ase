﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    class AseFullTextSearchFunctionsTranslator : IMethodCallTranslator
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

        private static readonly IDictionary<MethodInfo, string> _functionMapping
            = new Dictionary<MethodInfo, string>
            {
                { _freeTextMethodInfo, FreeTextFunctionName },
                { _freeTextMethodInfoWithLanguage, FreeTextFunctionName },
                { _containsMethodInfo, ContainsFunctionName },
                { _containsMethodInfoWithLanguage, ContainsFunctionName }
            };

        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public AseFullTextSearchFunctionsTranslator(
            ISqlExpressionFactory sqlExpressionFactory)
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
            MethodInfo method,
            IReadOnlyList<SqlExpression> arguments,
            IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            if (_functionMapping.TryGetValue(method, out var functionName))
            {
                var propertyReference = arguments[1];
                if (!(propertyReference is ColumnExpression))
                {
                    throw new InvalidOperationException("The 'FreeText' method is not supported because the query has switched to client-evaluation. Inspect the log to determine which query expressions are triggering client-evaluation.");
                }

                var typeMapping = propertyReference.TypeMapping;
                var freeText = _sqlExpressionFactory.ApplyTypeMapping(arguments[2], typeMapping);

                var functionArguments = new List<SqlExpression> { propertyReference, freeText };

                if (arguments.Count == 4)
                {
                    functionArguments.Add(
                        _sqlExpressionFactory.Fragment($"LANGUAGE {((SqlConstantExpression)arguments[3]).Value}"));
                }

                return _sqlExpressionFactory.Function(
                    functionName,
                    functionArguments,
                    typeof(bool));
            }

            return null;
        }
    }
}
