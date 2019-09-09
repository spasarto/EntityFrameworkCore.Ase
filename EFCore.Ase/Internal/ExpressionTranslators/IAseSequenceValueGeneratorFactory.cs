using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.Ase.Internal.ExpressionTranslators
{
    public interface IAseSequenceValueGeneratorFactory
    {
        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        ValueGenerator Create(
            IProperty property,
            AseSequenceValueGeneratorState generatorState,
            IAseConnection connection,
            IRawSqlCommandBuilder rawSqlCommandBuilder,
            IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger);
    }

    public class AseSequenceValueGeneratorFactory : IAseSequenceValueGeneratorFactory
    {
        private readonly IAseUpdateSqlGenerator _sqlGenerator;

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public AseSequenceValueGeneratorFactory(
            IAseUpdateSqlGenerator sqlGenerator)
        {
            _sqlGenerator = sqlGenerator;
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual ValueGenerator Create(
            IProperty property,
            AseSequenceValueGeneratorState generatorState,
            IAseConnection connection,
            IRawSqlCommandBuilder rawSqlCommandBuilder,
            IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
        {
            var type = property.ClrType.UnwrapNullableType().UnwrapEnumType();

            if (type == typeof(long))
            {
                return new AseSequenceHiLoValueGenerator<long>(
                    rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
            }

            if (type == typeof(int))
            {
                return new AseSequenceHiLoValueGenerator<int>(
                    rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
            }

            if (type == typeof(decimal))
            {
                return new AseSequenceHiLoValueGenerator<decimal>(
                    rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
            }

            if (type == typeof(short))
            {
                return new AseSequenceHiLoValueGenerator<short>(
                    rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
            }

            if (type == typeof(byte))
            {
                return new AseSequenceHiLoValueGenerator<byte>(
                    rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
            }

            if (type == typeof(char))
            {
                return new AseSequenceHiLoValueGenerator<char>(
                    rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
            }

            if (type == typeof(ulong))
            {
                return new AseSequenceHiLoValueGenerator<ulong>(
                    rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
            }

            if (type == typeof(uint))
            {
                return new AseSequenceHiLoValueGenerator<uint>(
                    rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
            }

            if (type == typeof(ushort))
            {
                return new AseSequenceHiLoValueGenerator<ushort>(
                    rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
            }

            if (type == typeof(sbyte))
            {
                return new AseSequenceHiLoValueGenerator<sbyte>(
                    rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
            }

            throw new ArgumentException(
                CoreStrings.InvalidValueGeneratorFactoryProperty(
                    nameof(AseSequenceValueGeneratorFactory), property.Name, property.DeclaringEntityType.DisplayName()));
        }
    }
}
