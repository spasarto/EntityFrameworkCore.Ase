using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using System;

namespace EntityFrameworkCore.Ase.Internal
{
    internal class AseModificationCommandBatch : AffectedCountModificationCommandBatch
    {
        private const int DefaultNetworkPacketSizeBytes = 4096;
        private const int MaxScriptLength = 65536 * DefaultNetworkPacketSizeBytes / 2;
        private const int MaxParameterCount = 2100;
        private const int MaxRowCount = 1000;
        private int _parameterCount = 1; // Implicit parameter for the command text
        private readonly int _maxBatchSize = 100; //randomly chosen
        private int _commandsLeftToLengthCheck = 50;

        public AseModificationCommandBatch(IRelationalCommandBuilderFactory commandBuilderFactory, ISqlGenerationHelper sqlGenerationHelper, IUpdateSqlGenerator updateSqlGenerator, IRelationalValueBufferFactoryFactory valueBufferFactoryFactory) : base(commandBuilderFactory, sqlGenerationHelper, updateSqlGenerator, valueBufferFactoryFactory)
        {
        }

        protected override bool CanAddCommand(ModificationCommand modificationCommand)
        {
            if (ModificationCommands.Count >= _maxBatchSize)
            {
                return false;
            }

            var additionalParameterCount = CountParameters(modificationCommand);

            if (_parameterCount + additionalParameterCount >= MaxParameterCount)
            {
                return false;
            }

            _parameterCount += additionalParameterCount;
            return true;
        }

        protected override bool IsCommandTextValid()
        {
            if (--_commandsLeftToLengthCheck < 0)
            {
                var commandTextLength = GetCommandText().Length;
                if (commandTextLength >= MaxScriptLength)
                {
                    return false;
                }

                var averageCommandLength = commandTextLength / ModificationCommands.Count;
                var expectedAdditionalCommandCapacity = (MaxScriptLength - commandTextLength) / averageCommandLength;
                _commandsLeftToLengthCheck = Math.Max(1, expectedAdditionalCommandCapacity / 4);
            }

            return true;
        }

        private static int CountParameters(ModificationCommand modificationCommand)
        {
            var parameterCount = 0;
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var columnIndex = 0; columnIndex < modificationCommand.ColumnModifications.Count; columnIndex++)
            {
                var columnModification = modificationCommand.ColumnModifications[columnIndex];
                if (columnModification.UseCurrentValueParameter)
                {
                    parameterCount++;
                }

                if (columnModification.UseOriginalValueParameter)
                {
                    parameterCount++;
                }
            }

            return parameterCount;
        }
    }
}
