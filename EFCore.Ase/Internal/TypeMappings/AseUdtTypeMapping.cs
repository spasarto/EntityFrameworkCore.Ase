using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Ase.Internal.TypeMappings
{

    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class AseUdtTypeMapping : RelationalTypeMapping
    {
        private static Action<DbParameter, string> _udtTypeNameSetter;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public AseUdtTypeMapping(
            Type clrType,
            string storeType,
            Func<object, Expression> literalGenerator,
            StoreTypePostfix storeTypePostfix = StoreTypePostfix.None,
             string udtTypeName = null,
             ValueConverter converter = null,
             ValueComparer comparer = null,
             ValueComparer keyComparer = null,
            DbType? dbType = null,
            bool unicode = false,
            int? size = null,
            bool fixedLength = false,
            int? precision = null,
            int? scale = null)
            : base(
                new RelationalTypeMappingParameters(
                    new CoreTypeMappingParameters(
                        clrType, converter, comparer, keyComparer), storeType, storeTypePostfix, dbType, unicode, size, fixedLength, precision, scale))

        {
            LiteralGenerator = literalGenerator;
            UdtTypeName = udtTypeName ?? storeType;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected AseUdtTypeMapping(
            RelationalTypeMappingParameters parameters,
            Func<object, Expression> literalGenerator,
             string udtTypeName)
            : base(parameters)
        {
            LiteralGenerator = literalGenerator;
            UdtTypeName = udtTypeName ?? parameters.StoreType;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual string UdtTypeName { get; }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Func<object, Expression> LiteralGenerator { get; }

        /// <summary>
        ///     Creates a copy of this mapping.
        /// </summary>
        /// <param name="parameters"> The parameters for this mapping. </param>
        /// <returns> The newly created mapping. </returns>
        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
            => new AseUdtTypeMapping(parameters, LiteralGenerator, UdtTypeName);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override void ConfigureParameter(DbParameter parameter)
            => SetUdtTypeName(parameter);

        private void SetUdtTypeName(DbParameter parameter)
        {
            NonCapturingLazyInitializer.EnsureInitialized(
                ref _udtTypeNameSetter,
                parameter.GetType(),
                CreateUdtTypeNameAccessor);

            if (parameter.Value != null
                && parameter.Value != DBNull.Value)
            {
                _udtTypeNameSetter(parameter, UdtTypeName);
            }
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override Expression GenerateCodeLiteral(object value)
            => LiteralGenerator(value);

        private static Action<DbParameter, string> CreateUdtTypeNameAccessor(Type paramType)
        {
            var paramParam = Expression.Parameter(typeof(DbParameter), "parameter");
            var valueParam = Expression.Parameter(typeof(string), "value");

            return Expression.Lambda<Action<DbParameter, string>>(
                Expression.Call(
                    Expression.Convert(paramParam, paramType),
                    paramType.GetProperty("UdtTypeName").SetMethod,
                    valueParam),
                paramParam,
                valueParam).Compile();
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public static AseUdtTypeMapping CreateSqlHierarchyIdMapping(Type udtType)
            => new AseUdtTypeMapping(
                udtType,
                "hierarchyid",
                v => Expression.Call(
                    v.GetType().GetMethod("Parse"),
                    Expression.New(
                        typeof(SqlString).GetConstructor(new[] { typeof(string) }),
                        Expression.Constant(v.ToString(), typeof(string)))));

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public static AseUdtTypeMapping CreateSqlSpatialMapping(Type udtType, string storeName)
            => new AseUdtTypeMapping(
                udtType,
                storeName,
                v =>
                {
                    var spatialType = v.GetType();
                    var noParams = new object[0];

                    var wkt = ((SqlChars)spatialType.GetMethod("AsTextZM").Invoke(v, noParams)).ToSqlString().ToString();
                    var srid = ((SqlInt32)spatialType.GetMethod("get_STSrid").Invoke(v, noParams)).Value;

                    return Expression.Call(
                        spatialType.GetMethod("STGeomFromText"),
                        Expression.New(
                            typeof(SqlChars).GetConstructor(
                                new[] { typeof(SqlString) }),
                            Expression.New(
                                typeof(SqlString).GetConstructor(
                                    new[] { typeof(string) }),
                                Expression.Constant(wkt, typeof(string)))),
                        Expression.Constant(srid, typeof(int)));
                });
    }
}
