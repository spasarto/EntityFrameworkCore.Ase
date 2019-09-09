using EntityFrameworkCore.Ase.Internal;
using EntityFrameworkCore.Ase.Internal.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
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
                //.TryAdd<ISingletonOptions, IAseOptions>(p => p.GetService<IAseOptions>())

                // New Query Pipeline
                .TryAdd<IMethodCallTranslatorProvider, AseMethodCallTranslatorProvider>()
                .TryAdd<IMemberTranslatorProvider, AseMemberTranslatorProvider>()
                .TryAdd<IQuerySqlGeneratorFactory, AseQuerySqlGeneratorFactory>()
                //.TryAdd<IQueryTranslationPostprocessorFactory, AseQueryTranslationPostprocessorFactory>()
                .TryAdd<IRelationalSqlTranslatingExpressionVisitorFactory, AseSqlTranslatingExpressionVisitorFactory>()
                .TryAddProviderSpecificServices(
                    b => b
                        .TryAddSingleton<IAseValueGeneratorCache, AseValueGeneratorCache>()
                        //.TryAddSingleton<IAseOptions, AseOptions>()
                        .TryAddSingleton<IAseUpdateSqlGenerator, AseUpdateSqlGenerator>()
                        .TryAddSingleton<IAseSequenceValueGeneratorFactory, AseSequenceValueGeneratorFactory>()
                        .TryAddScoped<IAseConnection, EfAseConnection>());
            builder.TryAddCoreServices();

            return serviceCollection;
        }
    }
}
