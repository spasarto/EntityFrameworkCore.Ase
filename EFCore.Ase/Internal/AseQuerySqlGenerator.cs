using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityFrameworkCore.Ase.Internal
{
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

        protected override void GenerateLimitOffset(SelectExpression selectExpression)
        {
            
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
}
