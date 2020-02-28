using EntityFrameworkCore.Ase.Internal.TypeMappings;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EntityFrameworkCore.Ase.Internal
{
    internal class AseTypeMappingSource : RelationalTypeMappingSource
    {
        //private readonly RelationalTypeMapping _sqlVariant
        //    = new SqlVariantTypeMapping("sql_variant");

        private readonly FloatTypeMapping _real
            = new FloatTypeMapping("real");

        private readonly ByteTypeMapping _byte
            = new ByteTypeMapping("tinyint", DbType.Byte);

        private readonly ShortTypeMapping _short
            = new ShortTypeMapping("smallint", DbType.Int16);

        private readonly LongTypeMapping _long
            = new LongTypeMapping("bigint", DbType.Int64);

        private readonly ByteArrayTypeMapping _rowversion
            = new AseByteArrayTypeMapping(
                "rowversion",
                size: 8,
                comparer: new ValueComparer<byte[]>(
                    (v1, v2) => StructuralComparisons.StructuralEqualityComparer.Equals(v1, v2),
                    v => StructuralComparisons.StructuralEqualityComparer.GetHashCode(v),
                    v => v == null ? null : v.ToArray()),
                storeTypePostfix: StoreTypePostfix.None);

        private readonly IntTypeMapping _int
            = new IntTypeMapping("int", DbType.Int32);

        private readonly BoolTypeMapping _bool
            = new BoolTypeMapping("bit");

        private readonly StringTypeMapping _fixedLengthUnicodeString
            = new AseStringTypeMapping(unicode: true, fixedLength: true);

        private readonly StringTypeMapping _variableLengthUnicodeString
            = new AseStringTypeMapping(unicode: true);

        private readonly StringTypeMapping _variableLengthMaxUnicodeString
            = new AseStringTypeMapping("nvarchar(max)", unicode: true, storeTypePostfix: StoreTypePostfix.None);

        private readonly StringTypeMapping _fixedLengthAnsiString
            = new AseStringTypeMapping(fixedLength: true);

        private readonly StringTypeMapping _variableLengthAnsiString
            = new AseStringTypeMapping();

        private readonly StringTypeMapping _variableLengthMaxAnsiString
            = new AseStringTypeMapping("varchar(max)", storeTypePostfix: StoreTypePostfix.None);

        private readonly ByteArrayTypeMapping _variableLengthBinary
            = new AseByteArrayTypeMapping();

        private readonly ByteArrayTypeMapping _variableLengthMaxBinary
            = new AseByteArrayTypeMapping("varbinary(max)", storeTypePostfix: StoreTypePostfix.None);

        private readonly ByteArrayTypeMapping _fixedLengthBinary
            = new AseByteArrayTypeMapping(fixedLength: true);

        private readonly DateTimeTypeMapping _date
            = new DateTimeTypeMapping("date", DbType.Date);

        private readonly DateTimeTypeMapping _datetime
            = new DateTimeTypeMapping("datetime", DbType.DateTime);

        private readonly DateTimeTypeMapping _datetime2
            = new DateTimeTypeMapping("datetime2", DbType.DateTime2);

        private readonly DoubleTypeMapping _double
            = new DoubleTypeMapping("float");

        private readonly DateTimeOffsetTypeMapping _datetimeoffset
            = new DateTimeOffsetTypeMapping("datetimeoffset");

        private readonly AseGuidTypeMapping _uniqueidentifier
            = new AseGuidTypeMapping(); //GuidTypeMapping("uniqueidentifier", DbType.Guid);

        private readonly DecimalTypeMapping _decimal
            = new AseDecimalTypeMapping("decimal(18, 2)", precision: 18, scale: 2, storeTypePostfix: StoreTypePostfix.PrecisionAndScale);

        private readonly DecimalTypeMapping _money
            = new AseDecimalTypeMapping("money");

        private readonly TimeSpanTypeMapping _time
            = new TimeSpanTypeMapping("time");

        private readonly StringTypeMapping _xml
            = new AseStringTypeMapping("xml", unicode: true, storeTypePostfix: StoreTypePostfix.None);

        private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;

        private readonly Dictionary<string, RelationalTypeMapping> _storeTypeMappings;

        // These are disallowed only if specified without any kind of length specified in parenthesis.
        private readonly HashSet<string> _disallowedMappings
            = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "binary",
                "binary varying",
                "varbinary",
                "char",
                "character",
                "char varying",
                "character varying",
                "varchar",
                "national char",
                "national character",
                "nchar",
                "national char varying",
                "national character varying",
                "nvarchar",
                "image",
                "sql_variant"
            };

        private readonly IReadOnlyDictionary<string, Func<Type, RelationalTypeMapping>> _namedClrMappings
            = new Dictionary<string, Func<Type, RelationalTypeMapping>>(StringComparer.Ordinal)
            {
                { "Microsoft..Types.SqlHierarchyId", t => AseUdtTypeMapping.CreateSqlHierarchyIdMapping(t) },
                { "Microsoft..Types.SqlGeography", t => AseUdtTypeMapping.CreateSqlSpatialMapping(t, "geography") },
                { "Microsoft..Types.SqlGeometry", t => AseUdtTypeMapping.CreateSqlSpatialMapping(t, "geometry") }
            };

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public AseTypeMappingSource(TypeMappingSourceDependencies dependencies, RelationalTypeMappingSourceDependencies relationalDependencies)
            : base(dependencies, relationalDependencies)
        {
            _clrTypeMappings
                = new Dictionary<Type, RelationalTypeMapping>
                {
                    { typeof(int), _int },
                    { typeof(long), _long },
                    { typeof(DateTime), _datetime2 },
                    { typeof(Guid), _uniqueidentifier },
                    { typeof(bool), _bool },
                    { typeof(byte), _byte },
                    { typeof(double), _double },
                    { typeof(DateTimeOffset), _datetimeoffset },
                    { typeof(short), _short },
                    { typeof(float), _real },
                    { typeof(decimal), _decimal },
                    { typeof(TimeSpan), _time }
                };

            _storeTypeMappings
                = new Dictionary<string, RelationalTypeMapping>(StringComparer.OrdinalIgnoreCase)
                {
                    { "bigint", _long },
                    { "binary varying", _variableLengthBinary },
                    { "binary", _fixedLengthBinary },
                    { "bit", _bool },
                    { "char varying", _variableLengthAnsiString },
                    { "char", _fixedLengthAnsiString },
                    { "character varying", _variableLengthAnsiString },
                    { "character", _fixedLengthAnsiString },
                    { "date", _date },
                    { "datetime", _datetime },
                    { "datetime2", _datetime2 },
                    { "datetimeoffset", _datetimeoffset },
                    { "dec", _decimal },
                    { "decimal", _decimal },
                    { "double precision", _double },
                    { "float", _double },
                    { "image", _variableLengthBinary },
                    { "int", _int },
                    { "money", _money },
                    { "national char varying", _variableLengthUnicodeString },
                    { "national character varying", _variableLengthUnicodeString },
                    { "national character", _fixedLengthUnicodeString },
                    { "nchar", _fixedLengthUnicodeString },
                    { "ntext", _variableLengthUnicodeString },
                    { "numeric", _decimal },
                    { "nvarchar", _variableLengthUnicodeString },
                    { "nvarchar(max)", _variableLengthMaxUnicodeString },
                    { "real", _real },
                    { "rowversion", _rowversion },
                    { "smalldatetime", _datetime },
                    { "smallint", _short },
                    { "smallmoney", _money },
                    //{ "sql_variant", _sqlVariant },
                    { "text", _variableLengthAnsiString },
                    { "time", _time },
                    { "timestamp", _rowversion },
                    { "tinyint", _byte },
                    { "uniqueidentifier", _uniqueidentifier },
                    { "varbinary", _variableLengthBinary },
                    { "varbinary(max)", _variableLengthMaxBinary },
                    { "varchar", _variableLengthAnsiString },
                    { "varchar(max)", _variableLengthMaxAnsiString },
                    { "xml", _xml }
                };
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override void ValidateMapping(CoreTypeMapping mapping, IProperty property)
        {
            var relationalMapping = mapping as RelationalTypeMapping;

            if (_disallowedMappings.Contains(relationalMapping?.StoreType))
            {
                if (property == null)
                {
                    throw new ArgumentException("");
                }

                throw new ArgumentException("");
            }
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo)
            => FindRawMapping(mappingInfo)?.Clone(mappingInfo)
               ?? base.FindMapping(mappingInfo);

        private RelationalTypeMapping FindRawMapping(RelationalTypeMappingInfo mappingInfo)
        {
            var clrType = mappingInfo.ClrType;
            var storeTypeName = mappingInfo.StoreTypeName;
            var storeTypeNameBase = mappingInfo.StoreTypeNameBase;

            if (storeTypeName != null)
            {
                if (clrType == typeof(float)
                    && mappingInfo.Size != null
                    && mappingInfo.Size <= 24
                    && (storeTypeNameBase.Equals("float", StringComparison.OrdinalIgnoreCase)
                        || storeTypeNameBase.Equals("double precision", StringComparison.OrdinalIgnoreCase)))
                {
                    return _real;
                }

                if (_storeTypeMappings.TryGetValue(storeTypeName, out var mapping)
                    || _storeTypeMappings.TryGetValue(storeTypeNameBase, out mapping))
                {
                    return clrType == null
                           || mapping.ClrType == clrType
                        ? mapping
                        : null;
                }
            }

            if (clrType != null)
            {
                if (_clrTypeMappings.TryGetValue(clrType, out var mapping))
                {
                    return mapping;
                }

                if (_namedClrMappings.TryGetValue(clrType.FullName, out var mappingFunc))
                {
                    return mappingFunc(clrType);
                }

                if (clrType == typeof(string))
                {
                    var isAnsi = mappingInfo.IsUnicode == false;
                    var isFixedLength = mappingInfo.IsFixedLength == true;
                    var maxSize = isAnsi ? 8000 : 4000;

                    var size = mappingInfo.Size ?? (mappingInfo.IsKeyOrIndex ? (int?)(isAnsi ? 900 : 450) : null);
                    if (size > maxSize)
                    {
                        size = isFixedLength ? maxSize : (int?)null;
                    }

                    return size == null
                        ? isAnsi ? _variableLengthMaxAnsiString : _variableLengthMaxUnicodeString
                        : new AseStringTypeMapping(
                            unicode: !isAnsi,
                            size: size,
                            fixedLength: isFixedLength);
                }

                if (clrType == typeof(byte[]))
                {
                    if (mappingInfo.IsRowVersion == true)
                    {
                        return _rowversion;
                    }

                    var isFixedLength = mappingInfo.IsFixedLength == true;

                    var size = mappingInfo.Size ?? (mappingInfo.IsKeyOrIndex ? (int?)900 : null);
                    if (size > 8000)
                    {
                        size = isFixedLength ? 8000 : (int?)null;
                    }

                    return size == null
                        ? _variableLengthMaxBinary
                        : new AseByteArrayTypeMapping(size: size, fixedLength: isFixedLength);
                }
            }

            return null;
        }
    }
}
