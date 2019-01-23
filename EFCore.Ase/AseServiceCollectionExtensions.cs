using EntityFrameworkCore.Ase.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Query.Sql;
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
                .TryAdd<IDatabaseProvider, DatabaseProvider<AseOptionsExtension>>()
                .TryAdd<IValueGeneratorCache>(s => s.GetService<IAseValueGeneratorCache>())
                .TryAdd<IRelationalTypeMappingSource, AseTypeMappingSource>()
                .TryAdd<ISqlGenerationHelper, AseSqlGenerationHelper>()
                //.TryAdd<IMigrationsAnnotationProvider, SqlServerMigrationsAnnotationProvider>()
                //.TryAdd<IModelValidator, SqlServerModelValidator>()
                //.TryAdd<IConventionSetBuilder, SqlServerConventionSetBuilder>()
                .TryAdd<IUpdateSqlGenerator>(p => p.GetService<IAseServerUpdateSqlGenerator>())
                .TryAdd<ISingletonUpdateSqlGenerator>(p => p.GetService<IAseServerUpdateSqlGenerator>())
                .TryAdd<IModificationCommandBatchFactory, AseModificationCommandBatchFactory>()
                //.TryAdd<IValueGeneratorSelector, SqlServerValueGeneratorSelector>()
                .TryAdd<IRelationalConnection>(s => s.GetService<IAseRelationalConnection>())
                //.TryAdd<IMigrationsSqlGenerator, SqlServerMigrationsSqlGenerator>()
                //.TryAdd<IRelationalDatabaseCreator, SqlServerDatabaseCreator>()
                //.TryAdd<IHistoryRepository, SqlServerHistoryRepository>()
                //.TryAdd<ICompiledQueryCacheKeyGenerator, SqlServerCompiledQueryCacheKeyGenerator>()
                //.TryAdd<IExecutionStrategyFactory, SqlServerExecutionStrategyFactory>()
                //.TryAdd<IQueryCompilationContextFactory, SqlServerQueryCompilationContextFactory>()
                .TryAdd<IMemberTranslator, AseCompositeMemberTranslator>()
                .TryAdd<ICompositeMethodCallTranslator, AseCompositeMethodCallTranslator>()
                .TryAdd<IQuerySqlGeneratorFactory, AseQuerySqlGeneratorFactory>()
                //.TryAdd<ISqlTranslatingExpressionVisitorFactory, SqlServerSqlTranslatingExpressionVisitorFactory>()
                //.TryAdd<ISingletonOptions, ISqlServerOptions>(p => p.GetService<ISqlServerOptions>())
                .TryAddProviderSpecificServices(
                    b => b
                        .TryAddSingleton<IAseValueGeneratorCache, AseValueGeneratorCache>()
                        //        .TryAddSingleton<ISqlServerOptions, SqlServerOptions>()
                        .TryAddSingleton<IAseServerUpdateSqlGenerator, AseServerUpdateSqlGenerator>()
                        //        .TryAddSingleton<ISqlServerSequenceValueGeneratorFactory, SqlServerSequenceValueGeneratorFactory>()
                        .TryAddScoped<IAseRelationalConnection, AseRelationalConnection>()
                        );

            builder.TryAddCoreServices();

            return serviceCollection;
        }
    }
}
