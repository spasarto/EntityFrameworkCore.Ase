using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Reflection;

namespace EntityFrameworkCore.Ase.Internal
{
    internal class AseQuerySqlGenerator : DefaultQuerySqlGenerator
    {
        private readonly Lazy<IRelationalCommandBuilder> _relationalCommandBuilder2;

        public AseQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies, SelectExpression selectExpression)
            : base(dependencies, selectExpression)
        {
            var relationalCommandBuilderField = typeof(DefaultQuerySqlGenerator).GetField("_relationalCommandBuilder", BindingFlags.Instance | BindingFlags.NonPublic);
            _relationalCommandBuilder2 = new Lazy<IRelationalCommandBuilder>(() => (IRelationalCommandBuilder)relationalCommandBuilderField.GetValue(this));
        }

        protected override void GenerateTop(SelectExpression selectExpression)
        {
            if (selectExpression.Limit != null
                && selectExpression.Offset == null)
            {
                _relationalCommandBuilder2.Value.Append("TOP ");

                Visit(selectExpression.Limit);

                _relationalCommandBuilder2.Value.Append(" ");
            }
        }
    }
}
