using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Ase.Internal
{
    public class AseCompiledQueryCacheKeyGenerator : RelationalCompiledQueryCacheKeyGenerator
    {/// <summary>
     ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
     ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
     ///     any release. You should only use it directly in your code with extreme caution and knowing that
     ///     doing so can result in application failures when updating to a new Entity Framework Core release.
     /// </summary>
        public AseCompiledQueryCacheKeyGenerator(
             CompiledQueryCacheKeyGeneratorDependencies dependencies,
             RelationalCompiledQueryCacheKeyGeneratorDependencies relationalDependencies)
            : base(dependencies, relationalDependencies)
        {
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public override object GenerateCacheKey(Expression query, bool async)
            => new AseCompiledQueryCacheKey(
                GenerateCacheKeyCore(query, async),
                false);

        private readonly struct AseCompiledQueryCacheKey
        {
            private readonly RelationalCompiledQueryCacheKey _relationalCompiledQueryCacheKey;
            private readonly bool _useRowNumberOffset;

            public AseCompiledQueryCacheKey(
                RelationalCompiledQueryCacheKey relationalCompiledQueryCacheKey, bool useRowNumberOffset)
            {
                _relationalCompiledQueryCacheKey = relationalCompiledQueryCacheKey;
                _useRowNumberOffset = useRowNumberOffset;
            }

            public override bool Equals(object obj)
                => !(obj is null)
                   && obj is AseCompiledQueryCacheKey
                   && Equals((AseCompiledQueryCacheKey)obj);

            private bool Equals(AseCompiledQueryCacheKey other)
                => _relationalCompiledQueryCacheKey.Equals(other._relationalCompiledQueryCacheKey)
                   && _useRowNumberOffset == other._useRowNumberOffset;

            public override int GetHashCode() => HashCode.Combine(_relationalCompiledQueryCacheKey, _useRowNumberOffset);
        }
    }
}
