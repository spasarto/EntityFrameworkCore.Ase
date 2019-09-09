using EntityFrameworkCore.Ase.Internal.ExpressionTranslators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Ase.Internal
{
    public class AseConventionSetBuilder : RelationalConventionSetBuilder
    {
        private readonly ISqlGenerationHelper _sqlGenerationHelper;

        /// <summary>
        ///     Creates a new <see cref="AseConventionSetBuilder" /> instance.
        /// </summary>
        /// <param name="dependencies"> The core dependencies for this service. </param>
        /// <param name="relationalDependencies"> The relational dependencies for this service. </param>
        /// <param name="sqlGenerationHelper"> The SQL generation helper to use. </param>
        public AseConventionSetBuilder(
             ProviderConventionSetBuilderDependencies dependencies,
             RelationalConventionSetBuilderDependencies relationalDependencies,
             ISqlGenerationHelper sqlGenerationHelper)
            : base(dependencies, relationalDependencies)
        {
            _sqlGenerationHelper = sqlGenerationHelper;
        }

        /// <summary>
        ///     Builds and returns the convention set for the current database provider.
        /// </summary>
        /// <returns> The convention set for the current database provider. </returns>
        public override ConventionSet CreateConventionSet()
        {
            var conventionSet = base.CreateConventionSet();

            var valueGenerationStrategyConvention = new AseValueGenerationStrategyConvention(Dependencies, RelationalDependencies);
            conventionSet.ModelInitializedConventions.Add(valueGenerationStrategyConvention);
            conventionSet.ModelInitializedConventions.Add(
                new RelationalMaxIdentifierLengthConvention(128, Dependencies, RelationalDependencies));

            //ValueGenerationConvention valueGenerationConvention =
            //    new AseValueGenerationConvention(Dependencies, RelationalDependencies);
            //ReplaceConvention(conventionSet.EntityTypeBaseTypeChangedConventions, valueGenerationConvention);

            //var AseInMemoryTablesConvention = new AseMemoryOptimizedTablesConvention(Dependencies, RelationalDependencies);
            //conventionSet.EntityTypeAnnotationChangedConventions.Add(AseInMemoryTablesConvention);

            //ReplaceConvention(conventionSet.EntityTypePrimaryKeyChangedConventions, valueGenerationConvention);

            //conventionSet.KeyAddedConventions.Add(AseInMemoryTablesConvention);

            //ReplaceConvention(conventionSet.ForeignKeyAddedConventions, valueGenerationConvention);

            //ReplaceConvention(conventionSet.ForeignKeyRemovedConventions, valueGenerationConvention);

            var aseIndexConvention = new AseIndexConvention(Dependencies, RelationalDependencies, _sqlGenerationHelper);

            conventionSet.EntityTypeBaseTypeChangedConventions.Add(aseIndexConvention);

            ConventionSet.AddBefore(
                conventionSet.ModelFinalizedConventions,
                valueGenerationStrategyConvention,
                typeof(ValidatingConvention));

            //conventionSet.IndexAddedConventions.Add(AseInMemoryTablesConvention);
            conventionSet.IndexAddedConventions.Add(aseIndexConvention);

            conventionSet.IndexUniquenessChangedConventions.Add(aseIndexConvention);

            conventionSet.IndexAnnotationChangedConventions.Add(aseIndexConvention);

            conventionSet.PropertyNullabilityChangedConventions.Add(aseIndexConvention);

            //StoreGenerationConvention storeGenerationConvention =
            //    new AseStoreGenerationConvention(Dependencies, RelationalDependencies);
            conventionSet.PropertyAnnotationChangedConventions.Add(aseIndexConvention);
            //ReplaceConvention(conventionSet.PropertyAnnotationChangedConventions, storeGenerationConvention);
            //ReplaceConvention(
            //    conventionSet.PropertyAnnotationChangedConventions, (RelationalValueGenerationConvention)valueGenerationConvention);

            //ReplaceConvention(conventionSet.ModelFinalizedConventions, storeGenerationConvention);

            return conventionSet;
        }

        /// <summary>
        ///     <para>
        ///         Call this method to build a <see cref="ConventionSet" /> for SQL Server when using
        ///         the <see cref="ModelBuilder" /> outside of <see cref="DbContext.OnModelCreating" />.
        ///     </para>
        ///     <para>
        ///         Note that it is unusual to use this method.
        ///         Consider using <see cref="DbContext" /> in the normal way instead.
        ///     </para>
        /// </summary>
        /// <returns> The convention set. </returns>
        public static ConventionSet Build()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkAse()
                .AddDbContext<DbContext>(
                    (p, o) =>
                        o.UseAse("Server=.")
                            .UseInternalServiceProvider(p))
                .BuildServiceProvider();

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DbContext>())
                {
                    return ConventionSet.CreateConventionSet(context);
                }
            }
        }
    }
}
