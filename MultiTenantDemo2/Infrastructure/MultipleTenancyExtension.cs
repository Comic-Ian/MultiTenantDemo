using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace MultiTenantDemo2.Infrastructure
{
    public static class MultipleTenancyExtension
    {
        public static IServiceCollection AddDatabasePerConnection<TDbContext>(this IServiceCollection services,
            string key = "default")
            where TDbContext : DbContext, ITenantDbContext
        {
            var option = new ConnectionResolverOption()
            {
                Key = key,
                Type = ConnectionResolverType.ByDatabase
            };
            return services.AddDatabasePerConnection<TDbContext>(option);
        }

        public static IServiceCollection AddDatabasePerConnection<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            if (option == null)
            {
                option = new ConnectionResolverOption()
                {
                    Key = "default",
                    Type = ConnectionResolverType.ByDatabase,
                };
            }

            return services.AddDatabase<TDbContext>(option);
        }
        //TenantModelCacheKeyFactory的作用主要是创建TenantModelCacheKey实例。TenantModelCacheKey的作用是作为一个键值，标识dbContext中的OnModelCreating否需要调用。
        //为什么这样做呢？因为ef core为了优化效率，避免在dbContext每次实例化的时候，都需要重新构建数据实体模型。
        //在默认情况下，OnModelCreating只会调用一次就会存在缓存。但由于我们创建了TenantModelCacheKey，使得我们有机会判断在什么情况下需要重新调用OnModelCreating
        internal static IServiceCollection AddDatabase<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            //容器的三部曲：实例化一个容器、注册、获取服务
            // 单例生命周期  在容器中永远只有当前这一个 AddSingleton
            services.AddSingleton(option);

            //当前请求作用域内  只有当前这个实例 AddScoped
            services.AddScoped<TenantInfo>();
            services.AddScoped<ISqlConnectionResolver, TenantSqlConnectionResolver>();
            services.AddDbContext<TDbContext>((serviceProvider, options) =>
            {
                var resolver = serviceProvider.GetRequiredService<ISqlConnectionResolver>();

                var dbOptionBuilder = options.UseSqlServer(resolver.GetConnection());
                // TenantModelCacheKeyFactory 配置到dbOptionBuilder
                if (option.Type == ConnectionResolverType.ByTabel)
                {
                    dbOptionBuilder.ReplaceService<IModelCacheKeyFactory, TenantModelCacheKeyFactory<TDbContext>>();
                }
            });

            return services;
        }

        public static IServiceCollection AddTenantDatabasePerTable<TDbContext>(this IServiceCollection services,
                string connectionStringName, string key = "default")
            where TDbContext : DbContext, ITenantDbContext
        {
            var option = new ConnectionResolverOption()
            {
                Key = key,
                Type = ConnectionResolverType.ByTabel,
                ConnectinStringName = connectionStringName
            };

            return services.AddTenantDatabasePerTable<TDbContext>(option);
        }

        public static IServiceCollection AddTenantDatabasePerTable<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            if (option == null)
            {
                option = new ConnectionResolverOption()
                {
                    Key = "default",
                    Type = ConnectionResolverType.ByTabel,
                    ConnectinStringName = "default"
                };
            }


            return services.AddDatabase<TDbContext>(option);
        }
    }
}
