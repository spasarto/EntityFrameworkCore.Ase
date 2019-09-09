using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    public class AseSqlTranslatingExpressionVisitorFactory : IRelationalSqlTranslatingExpressionVisitorFactory
    {
        private readonly RelationalSqlTranslatingExpressionVisitorDependencies _dependencies;

        public AseSqlTranslatingExpressionVisitorFactory(
            RelationalSqlTranslatingExpressionVisitorDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public virtual RelationalSqlTranslatingExpressionVisitor Create(
            IModel model,
            QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
            => new AseSqlTranslatingExpressionVisitor(
                _dependencies,
                model,
                queryableMethodTranslatingExpressionVisitor);
    }
}
