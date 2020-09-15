using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantDemo.DAL;

namespace MultiTenantDemo.Infrastructure
{
    //里面包含最重要的配置数据库连接字符串的方法。其中里面的DbContext并没有使用泛型，是为了更加简明点
    public static class MultipleTenancyExtension
    {
        public static IServiceCollection AddConnectionByDatabase(this IServiceCollection services)
        {
            services.AddDbContext<StoreDbContext>((serviceProvider, options) =>
            {
                var resolver = serviceProvider.GetRequiredService<ISqlConnectionResolver>();

                options.UseSqlServer(resolver.GetConnection());
            });

            return services;
        }
    }
}
