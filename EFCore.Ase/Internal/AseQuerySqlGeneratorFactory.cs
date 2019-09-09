using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.Ase.Internal
{
    internal class AseQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies _dependencies;

        public AseQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public virtual QuerySqlGenerator Create()
            => new AseQuerySqlGenerator(_dependencies);
    }
}
