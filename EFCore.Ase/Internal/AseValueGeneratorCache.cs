using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace EntityFrameworkCore.Ase.Internal
{
    internal interface IAseValueGeneratorCache : IValueGeneratorCache
    {

    }

    internal class AseValueGeneratorCache : ValueGeneratorCache, IAseValueGeneratorCache
    {
        private readonly ConcurrentDictionary<string, AseSequenceValueGeneratorState> _sequenceGeneratorCache
            = new ConcurrentDictionary<string, AseSequenceValueGeneratorState>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueGeneratorCache" /> class.
        /// </summary>
        /// <param name="dependencies"> Parameter object containing dependencies for this service. </param>
        public AseValueGeneratorCache(ValueGeneratorCacheDependencies dependencies)
            : base(dependencies)
        {
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual AseSequenceValueGeneratorState GetOrAddSequenceState(
            IProperty property,
            IRelationalConnection connection)
        {
            ISequence sequence = null; // property.Ase().FindHiLoSequence(); TODO https://github.com/aspnet/EntityFrameworkCore/blob/master/src/EFCore.Ase/Metadata/AsePropertyAnnotations.cs

            Debug.Assert(sequence != null);

            return _sequenceGeneratorCache.GetOrAdd(
                GetSequenceName(sequence, connection),
                sequenceName => new AseSequenceValueGeneratorState(sequence));
        }

        private static string GetSequenceName(ISequence sequence, IRelationalConnection connection)
        {
            var dbConnection = connection.DbConnection;

            return dbConnection.Database.ToUpperInvariant()
                   + "::"
                   + dbConnection.DataSource?.ToUpperInvariant()
                   + "::"
                   + (sequence.Schema == null ? "" : sequence.Schema + ".") + sequence.Name;
        }
    }
}