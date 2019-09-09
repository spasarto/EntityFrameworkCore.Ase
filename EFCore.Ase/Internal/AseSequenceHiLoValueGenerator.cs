using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Ase.Internal
{
    public class AseSequenceHiLoValueGenerator<TValue> : HiLoValueGenerator<TValue>
    {
        private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;
        private readonly IAseUpdateSqlGenerator _sqlGenerator;
        private readonly IAseConnection _connection;
        private readonly ISequence _sequence;
        private readonly IDiagnosticsLogger<DbLoggerCategory.Database.Command> _commandLogger;

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public AseSequenceHiLoValueGenerator(
             IRawSqlCommandBuilder rawSqlCommandBuilder,
             IAseUpdateSqlGenerator sqlGenerator,
             AseSequenceValueGeneratorState generatorState,
             IAseConnection connection,
             IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
            : base(generatorState)
        {
            _sequence = generatorState.Sequence;
            _rawSqlCommandBuilder = rawSqlCommandBuilder;
            _sqlGenerator = sqlGenerator;
            _connection = connection;
            _commandLogger = commandLogger;
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        protected override long GetNewLowValue()
            => (long)Convert.ChangeType(
                _rawSqlCommandBuilder
                    .Build(_sqlGenerator.GenerateNextSequenceValueOperation(_sequence.Name, _sequence.Schema))
                    .ExecuteScalar(
                        new RelationalCommandParameterObject(
                            _connection,
                            null,
                            null,
                            _commandLogger)),
                typeof(long),
                CultureInfo.InvariantCulture);

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        protected override async Task<long> GetNewLowValueAsync(CancellationToken cancellationToken = default)
            => (long)Convert.ChangeType(
                await _rawSqlCommandBuilder
                    .Build(_sqlGenerator.GenerateNextSequenceValueOperation(_sequence.Name, _sequence.Schema))
                    .ExecuteScalarAsync(
                        new RelationalCommandParameterObject(
                            _connection,
                            null,
                            null,
                            _commandLogger),
                        cancellationToken),
                typeof(long),
                CultureInfo.InvariantCulture);

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override bool GeneratesTemporaryValues => false;
    }
}
