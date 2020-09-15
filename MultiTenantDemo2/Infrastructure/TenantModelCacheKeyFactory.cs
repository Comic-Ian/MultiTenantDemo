using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MultiTenantDemo2.Infrastructure
{
    //1.  ModelCacheKeyFactory，这个是EF core提供的对象，主要是要来产生ModelCacheKey
    //2.  ModelCacheKey， 这个跟ModelCacheKeyFactory是一对的，如果需要自定义的话一般要同时实现他们
    //3.  ConnectionResolverOption，这个是项目自定义的对象，用于配置。因为我们项目中现在需要同时支持多种租户数据分离的方式

    //TenantModelCacheKeyFactory的作用主要是创建TenantModelCacheKey实例。TenantModelCacheKey的作用是作为一个键值，标识dbContext中的OnModelCreating否需要调用。
    //为什么这样做呢？因为ef core为了优化效率，避免在dbContext每次实例化的时候，都需要重新构建数据实体模型。
    //在默认情况下，OnModelCreating只会调用一次就会存在缓存。但由于我们创建了TenantModelCacheKey，使得我们有机会判断在什么情况下需要重新调用OnModelCreating
    internal sealed class TenantModelCacheKeyFactory<TContext> : ModelCacheKeyFactory
        where TContext : DbContext, ITenantDbContext
    {

        public override object Create(DbContext context)
        {   
            var dbContext = context as TContext;
            return new TenantModelCacheKey<TContext>(dbContext, dbContext?.TenantInfo?.Name ?? "no_tenant_identifier");
        }

        public TenantModelCacheKeyFactory(ModelCacheKeyFactoryDependencies dependencies) : base(dependencies)
        {
        }
    }

    internal sealed class TenantModelCacheKey<TContext> : ModelCacheKey
        where TContext : DbContext, ITenantDbContext
    {
        private readonly TContext _context;
        private readonly string _identifier;
        public TenantModelCacheKey(TContext context, string identifier) : base(context)
        {
            this._context = context;
            this._identifier = identifier;
        }

        protected override bool Equals(ModelCacheKey other)
        {
            return base.Equals(other) && (other as TenantModelCacheKey<TContext>)?._identifier == _identifier;
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            if (_identifier != null)
            {
                hashCode ^= _identifier.GetHashCode();
            }

            return hashCode;
        }
    }
}
