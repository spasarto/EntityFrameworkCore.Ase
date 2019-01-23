using AdoNetCore.AseClient;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace EntityFrameworkCore.Ase.Internal
{
    internal interface IAseRelationalConnection : IRelationalConnection
    {

    }

    internal class AseRelationalConnection : RelationalConnection, IAseRelationalConnection
    {
        public AseRelationalConnection(RelationalConnectionDependencies dependencies)
            : base(dependencies)
        {
        }

        protected override DbConnection CreateDbConnection() => new AseConnection(ConnectionString);
    }
}
