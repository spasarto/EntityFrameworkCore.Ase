using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using EntityFrameworkCore.Ase.Internal;
using EntityFrameworkCore.Ase.Internal.ExpressionTranslators;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Ase
{
    public static class AseServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkAse(this IServiceCollection serviceCollection)
        {
            var builder = new EntityFrameworkRelationalServicesBuilder(serviceCollection)
                                        /*
                                        .TryAdd<IDatabaseProvider, DatabaseProvider<AseOptionsExtension>>()
                                        .TryAdd<IValueGeneratorCache>(s => s.GetService<IAseValueGeneratorCache>())
                                        .TryAdd<IRelationalTypeMappingSource, AseTypeMappingSource>()
                                        .TryAdd<ISqlGenerationHelper, AseSqlGenerationHelper>()
                                        //.TryAdd<IMigrationsAnnotationProvider, AseMigrationsAnnotationProvider>()
                                        //.TryAdd<IModelValidator, AseModelValidator>()
                                        //.TryAdd<IConventionSetBuilder, AseConventionSetBuilder>()
                                        .TryAdd<IUpdateSqlGenerator>(p => p.GetService<IAseServerUpdateSqlGenerator>())
                                        .TryAdd<ISingletonUpdateSqlGenerator>(p => p.GetService<IAseServerUpdateSqlGenerator>())
                                        .TryAdd<IModificationCommandBatchFactory, AseModificationCommandBatchFactory>()
                                        //.TryAdd<IValueGeneratorSelector, AseValueGeneratorSelector>()
                                        .TryAdd<IRelationalConnection>(s => s.GetService<IAseRelationalConnection>())
                                        //.TryAdd<IMigrationsSqlGenerator, AseMigrationsSqlGenerator>()
                                        //.TryAdd<IRelationalDatabaseCreator, AseDatabaseCreator>()
                                        //.TryAdd<IHistoryRepository, AseHistoryRepository>()
                                        //.TryAdd<ICompiledQueryCacheKeyGenerator, AseCompiledQueryCacheKeyGenerator>()
                                        //.TryAdd<IExecutionStrategyFactory, AseExecutionStrategyFactory>()
                                        .TryAdd<IQueryCompilationContextFactory, AseQueryCompilationContextFactory>()
                                        .TryAdd<IMemberTranslator, AseCompositeMemberTranslator>()
                                        .TryAdd<ICompositeMethodCallTranslator, AseCompositeMethodCallTranslator>()
                                        .TryAdd<IQuerySqlGeneratorFactory, AseQuerySqlGeneratorFactory>()
                                        //.TryAdd<ISqlTranslatingExpressionVisitorFactory, AseSqlTranslatingExpressionVisitorFactory>()
                                        //.TryAdd<ISingletonOptions, IAseOptions>(p => p.GetService<IAseOptions>())
                                        .TryAddProviderSpecificServices(
                                            b => b
                                                .TryAddSingleton<IAseValueGeneratorCache, AseValueGeneratorCache>()
                                                //        .TryAddSingleton<IAseOptions, AseOptions>()
                                                .TryAddSingleton<IAseServerUpdateSqlGenerator, AseServerUpdateSqlGenerator>()
                                                //        .TryAddSingleton<IAseSequenceValueGeneratorFactory, AseSequenceValueGeneratorFactory>()
                                                .TryAddScoped<IAseRelationalConnection, AseRelationalConnection>()
                                                );
                                                */

                .TryAdd<LoggingDefinitions, AseLoggingDefinitions>()
                .TryAdd<IDatabaseProvider, DatabaseProvider<AseOptionsExtension>>()
                .TryAdd<IValueGeneratorCache>(p => p.GetService<IAseValueGeneratorCache>())
                .TryAdd<IRelationalTypeMappingSource, AseTypeMappingSource>()
                .TryAdd<ISqlGenerationHelper, AseSqlGenerationHelper>()
                //.TryAdd<IMigrationsAnnotationProvider, AseMigrationsAnnotationProvider>()
                //.TryAdd<IModelValidator, AseModelValidator>()
                .TryAdd<IProviderConventionSetBuilder, AseConventionSetBuilder>()
                .TryAdd<IUpdateSqlGenerator>(p => p.GetService<IAseUpdateSqlGenerator>())
                .TryAdd<IModificationCommandBatchFactory, AseModificationCommandBatchFactory>()
                .TryAdd<IValueGeneratorSelector, AseValueGeneratorSelector>()
                .TryAdd<IRelationalConnection>(p => p.GetService<IAseConnection>())
                //.TryAdd<IMigrationsSqlGenerator, AseMigrationsSqlGenerator>()
                //.TryAdd<IRelationalDatabaseCreator, AseDatabaseCreator>()
                //.TryAdd<IHistoryRepository, AseHistoryRepository>()
                .TryAdd<ICompiledQueryCacheKeyGenerator, AseCompiledQueryCacheKeyGenerator>()
                .TryAdd<IExecutionStrategyFactory, AseExecutionStrategyFactory>()
                .TryAdd<ISingletonOptions, IAseOptions>(p => p.GetService<IAseOptions>())

                .TryAdd<IQueryCompiler, AseQueryCompiler>()
                                
                // New Query Pipeline
                .TryAdd<IMethodCallTranslatorProvider, AseMethodCallTranslatorProvider>()
                .TryAdd<IMemberTranslatorProvider, AseMemberTranslatorProvider>()
                .TryAdd<IQuerySqlGeneratorFactory, AseQuerySqlGeneratorFactory>()
                //.TryAdd<IQueryTranslationPostprocessorFactory, AseQueryTranslationPostprocessorFactory>()
                .TryAdd<IRelationalSqlTranslatingExpressionVisitorFactory, AseSqlTranslatingExpressionVisitorFactory>()
                .TryAddProviderSpecificServices(
                    b => b
                        .TryAddSingleton<IAseValueGeneratorCache, AseValueGeneratorCache>()
                        .TryAddSingleton<IAseOptions, AseOptions>()
                        .TryAddSingleton<IAseUpdateSqlGenerator, AseUpdateSqlGenerator>()
                        .TryAddSingleton<IAseSequenceValueGeneratorFactory, AseSequenceValueGeneratorFactory>()
                        .TryAddScoped<IAseConnection, EfAseConnection>());
            builder.TryAddCoreServices();

            return serviceCollection;
        }
    }

    class AseQueryCompiler : QueryCompiler
    {
        public AseQueryCompiler( IQueryContextFactory queryContextFactory,  ICompiledQueryCache compiledQueryCache,  ICompiledQueryCacheKeyGenerator compiledQueryCacheKeyGenerator,  IDatabase database,  IDiagnosticsLogger<DbLoggerCategory.Query> logger,  ICurrentDbContext currentContext,  IEvaluatableExpressionFilter evaluatableExpressionFilter,  IModel model) : base(queryContextFactory, compiledQueryCache, compiledQueryCacheKeyGenerator, database, logger, currentContext, evaluatableExpressionFilter, model)
        {
        }

        public override Expression ExtractParameters(Expression query, IParameterValues parameterValues, IDiagnosticsLogger<DbLoggerCategory.Query> logger, bool parameterize = true, bool generateContextAccessors = false)
        {
            var visitor = new AseParameterExtractingExpressionVisitor(
                (IEvaluatableExpressionFilter)(typeof(QueryCompiler).GetField("_evaluatableExpressionFilter", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this)),
                parameterValues,
                (Type)(typeof(QueryCompiler).GetField("_contextType", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this)),
                (IModel)(typeof(QueryCompiler).GetField("_model", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this)),
                logger,
                parameterize,
                generateContextAccessors);

            return visitor.ExtractParameters(query);
        }
    }

    class AseParameterExtractingExpressionVisitor : ParameterExtractingExpressionVisitor
    {
        IDictionary<Expression, bool> _evaluatableExpressions;
        Stack<Expression> _constantExpressions = new Stack<Expression>();

        public AseParameterExtractingExpressionVisitor(IEvaluatableExpressionFilter evaluatableExpressionFilter,
            IParameterValues parameterValues, 
            Type contextType, 
            IModel model, 
            IDiagnosticsLogger<DbLoggerCategory.Query> logger, 
            bool parameterize,
            bool generateContextAccessors) 
            : base(evaluatableExpressionFilter, parameterValues, contextType, model, logger, parameterize, generateContextAccessors)
        {
            _evaluatableExpressions = (IDictionary<Expression, bool>)(typeof(ParameterExtractingExpressionVisitor).GetField("_evaluatableExpressions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this));
        }

        protected override Expression VisitConstant(ConstantExpression constantExpression)
        {
            return base.VisitConstant(constantExpression);
        }

        public override Expression Visit(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression
            && (methodCallExpression.Method.Name == "Take"
            || methodCallExpression.Method.Name == "Skip"))
            {
                _constantExpressions.Push(expression);
            }

            var b = base.Visit(expression);

            if (_constantExpressions.Count > 0
                && expression is ConstantExpression constantExpression
                && !(b is ConstantExpression))
            {
                _constantExpressions.Pop();
                return expression;
            }

            return b;
        }
    }
}
