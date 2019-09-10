using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.Ase.Internal
{
    internal class AseQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies _dependencies;
        private readonly IAseOptions _aseOptions;

        public AseQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies, IAseOptions aseOptions)
        {
            _dependencies = dependencies;
            _aseOptions = aseOptions;
        }

        public virtual QuerySqlGenerator Create()
            => new AseQuerySqlGenerator(_dependencies, _aseOptions);
    }
}
