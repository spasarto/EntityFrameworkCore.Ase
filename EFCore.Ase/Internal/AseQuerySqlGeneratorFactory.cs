using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.Sql;

namespace EntityFrameworkCore.Ase.Internal
{
    internal class AseQuerySqlGeneratorFactory : QuerySqlGeneratorFactoryBase
    {
        public AseQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies)
            : base(dependencies)
        {
        }

        public override IQuerySqlGenerator CreateDefault(SelectExpression selectExpression)
            => new AseQuerySqlGenerator(Dependencies, selectExpression);
    }
}
