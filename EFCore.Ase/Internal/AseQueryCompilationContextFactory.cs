using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.ResultOperators.Internal;

namespace EntityFrameworkCore.Ase.Internal
{
    public class AseQueryCompilationContextFactory : QueryCompilationContextFactory
    {
        public AseQueryCompilationContextFactory(
               QueryCompilationContextDependencies dependencies,
               RelationalQueryCompilationContextDependencies relationalDependencies)
               : base(dependencies)
        {
            relationalDependencies
                .NodeTypeProviderFactory
                .RegisterMethods(FromSqlExpressionNode.SupportedMethods, typeof(FromSqlExpressionNode));
        }

        public override QueryCompilationContext Create(bool async)
        {
            return async
                ? new AseQueryCompilationContext(
                    Dependencies,
                    new AsyncLinqOperatorProvider(),
                    new AsyncQueryMethodProvider(),
                    TrackQueryResults)
                : new AseQueryCompilationContext(
                    Dependencies,
                    new LinqOperatorProvider(),
                    new QueryMethodProvider(),
                    TrackQueryResults);
        }
    }

    public class AseQueryCompilationContext : RelationalQueryCompilationContext
    {
        public AseQueryCompilationContext(QueryCompilationContextDependencies dependencies, ILinqOperatorProvider linqOperatorProvider, IQueryMethodProvider queryMethodProvider, bool trackQueryResults)
            : base(dependencies, linqOperatorProvider, queryMethodProvider, trackQueryResults)
        {

        }

        public override int MaxTableAliasLength => 30;
    }
}
