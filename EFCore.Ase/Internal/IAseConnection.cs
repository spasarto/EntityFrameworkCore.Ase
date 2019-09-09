using AdoNetCore.AseClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace EntityFrameworkCore.Ase.Internal
{
    public interface IAseConnection : IRelationalConnection
    {
    }

    public class EfAseConnection : RelationalConnection, IAseConnection
    {
        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public EfAseConnection(RelationalConnectionDependencies dependencies)
            : base(dependencies)
        {
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        protected override DbConnection CreateDbConnection() => new AseConnection(ConnectionString);

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual IAseConnection CreateMasterConnection()
        {
            var contextOptions = new DbContextOptionsBuilder()
                .UseAse(ConnectionString)
                .Options;

            return new EfAseConnection(Dependencies.With(contextOptions));
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override bool IsMultipleActiveResultSetsEnabled
            => false;

        /// <summary>
        ///     Indicates whether the store connection supports ambient transactions
        /// </summary>
        protected override bool SupportsAmbientTransactions => true;
    }
}
