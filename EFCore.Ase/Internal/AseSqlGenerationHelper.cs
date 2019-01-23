using Microsoft.EntityFrameworkCore.Storage;
using System.Text;

namespace EntityFrameworkCore.Ase.Internal
{
    internal class AseSqlGenerationHelper : RelationalSqlGenerationHelper
    {
        public AseSqlGenerationHelper(RelationalSqlGenerationHelperDependencies dependencies)
            : base(dependencies)
        {
        }

        public override string DelimitIdentifier(string identifier)
        {
            return $"[{identifier}]";
        }

        public override string DelimitIdentifier(string name, string schema)
        {
            if (!string.IsNullOrEmpty(schema))
                return $"[{schema}].[{name}]";
            return DelimitIdentifier(name);
        }

        public override void DelimitIdentifier(StringBuilder builder, string identifier)
        {
            builder.Append($"[{identifier}]");
        }

        public override string StatementTerminator => "";
    }
}
