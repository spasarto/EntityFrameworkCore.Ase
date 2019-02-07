using Microsoft.EntityFrameworkCore.Update;
using System.Globalization;
using System.Text;

namespace EntityFrameworkCore.Ase.Internal
{
    public interface IAseServerUpdateSqlGenerator : IUpdateSqlGenerator
    {

    }

    internal class AseServerUpdateSqlGenerator : UpdateSqlGenerator, IAseServerUpdateSqlGenerator
    {
        public AseServerUpdateSqlGenerator(UpdateSqlGeneratorDependencies dependencies)
            : base(dependencies)
        {
        }

        protected override void AppendIdentityWhereCondition(StringBuilder commandStringBuilder, ColumnModification columnModification)
        {
            SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, columnModification.ColumnName);
            commandStringBuilder.Append(" = ");

            commandStringBuilder.Append("@@IDENTITY");
        }

        protected override void AppendRowsAffectedWhereCondition(StringBuilder commandStringBuilder, int expectedRowsAffected)
            => commandStringBuilder
                .Append("@@ROWCOUNT = ")
                .Append(expectedRowsAffected.ToString(CultureInfo.InvariantCulture));
    }
}