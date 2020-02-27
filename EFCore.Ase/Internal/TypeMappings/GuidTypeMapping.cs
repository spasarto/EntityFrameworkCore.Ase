using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace EntityFrameworkCore.Ase.Internal.TypeMappings
{
    public class AseGuidTypeMapping : RelationalTypeMapping
    {
        public AseGuidTypeMapping()
            : base(new RelationalTypeMappingParameters(
                    new CoreTypeMappingParameters(
                        typeof(Guid),
                        new GuidToStringConverter(),
                        new ValueComparer<Guid>(false),
                        new ValueComparer<Guid>(false)),
                        "varchar",
                        StoreTypePostfix.Size,
                        System.Data.DbType.Guid,
                        false,
                        36,
                        true,
                        null,
                        null))
        {
        }

        protected override string GenerateNonNullSqlLiteral(object value)
            => IsUnicode
                ? $"N'{value.ToString()}'" // Interpolation okay; strings
                : $"'{value.ToString()}'";
    }
}
