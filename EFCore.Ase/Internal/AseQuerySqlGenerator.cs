using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Ase.Internal
{
    public class AseQuerySqlGenerator : QuerySqlGenerator
    {
        public AseQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies)
            : base(dependencies)
        {
        }

        protected override void GenerateTop(SelectExpression selectExpression)
        {
            if (selectExpression.Limit != null
                && selectExpression.Offset == null)
            {
                Sql.Append("TOP ");

                Visit(selectExpression.Limit);

                Sql.Append(" ");
            }
        }

        protected override void GenerateLimitOffset(SelectExpression selectExpression)
        {
            // Note: For Limit without Offset, SqlServer generates TOP()
            if (selectExpression.Offset != null)
            {
                Sql.AppendLine()
                    .Append("OFFSET ");

                Visit(selectExpression.Offset);

                Sql.Append(" ROWS");

                if (selectExpression.Limit != null)
                {
                    Sql.Append(" FETCH NEXT ");

                    Visit(selectExpression.Limit);

                    Sql.Append(" ROWS ONLY");
                }
            }
        }

        protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
        {
            if (!sqlFunctionExpression.IsBuiltIn
                && string.IsNullOrEmpty(sqlFunctionExpression.Schema))
            {
                sqlFunctionExpression = SqlFunctionExpression.Create(
                    schema: "dbo",
                    sqlFunctionExpression.Name,
                    sqlFunctionExpression.Arguments,
                    sqlFunctionExpression.Type,
                    sqlFunctionExpression.TypeMapping);
            }

            return base.VisitSqlFunction(sqlFunctionExpression);
        }
    }
    /*
    internal class AseQuerySqlGenerator : DefaultQuerySqlGenerator
    {
        private readonly Lazy<IRelationalCommandBuilder> _relationalCommandBuilder2;
        private int _levels;

        public AseQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies, SelectExpression selectExpression)
            : base(dependencies, selectExpression)
        {
            var relationalCommandBuilderField = typeof(DefaultQuerySqlGenerator).GetField("_relationalCommandBuilder", BindingFlags.Instance | BindingFlags.NonPublic);
            _relationalCommandBuilder2 = new Lazy<IRelationalCommandBuilder>(() => (IRelationalCommandBuilder)relationalCommandBuilderField.GetValue(this));
        }

        protected override void GenerateTop(SelectExpression selectExpression)
        {
            if (selectExpression.Limit != null)
            {
                _relationalCommandBuilder2.Value.Append("TOP ");

                if (selectExpression.Limit is ConstantExpression constantExpression)
                {
                    _relationalCommandBuilder2.Value.Append(constantExpression.Value);
                }
                else if(ParameterValues.TryGetValue(selectExpression.Limit.ToString(), out object top))
                {
                    _relationalCommandBuilder2.Value.Append(top);
                }

                _relationalCommandBuilder2.Value.Append(" ");
            }

            if (selectExpression.Offset != null)
            {
                _relationalCommandBuilder2.Value.Append("START AT ");

                if (selectExpression.Offset is ConstantExpression constantExpression)
                {
                    _relationalCommandBuilder2.Value.Append(constantExpression.Value);
                }
                else if(ParameterValues.TryGetValue(selectExpression.Offset.ToString(), out object offset))
                {
                    _relationalCommandBuilder2.Value.Append(offset);
                }

                _relationalCommandBuilder2.Value.Append(" ");
            }
        }

        public override IRelationalCommand GenerateSql(IReadOnlyDictionary<string, object> parameterValues)
        {
            _levels = 0;
            var r = base.GenerateSql(parameterValues);
            return r;
        }
        
        public override Expression VisitSelect(SelectExpression selectExpression)
        {
            if (++_levels > 1)
            {
                selectExpression.ClearOrderBy();
            }

            var v = base.VisitSelect(selectExpression);

            _levels--;
            return v;
        }
    }
    */
}
