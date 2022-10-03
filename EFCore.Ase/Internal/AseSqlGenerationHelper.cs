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
            // ASE 12.5.4 complains about getting too long an identifier if the column name happens to be longer than 28.
            if (identifier.Length > MaxIdentifierLength - DelimitersToInsert)
            {
                builder.Append($"{identifier}");
                return;
            }
            builder.Append($"[{identifier}]");
        }

        public override string StatementTerminator => "";

        private const int MaxIdentifierLength = 30;
        private const int DelimitersToInsert = 2;
    }
}
