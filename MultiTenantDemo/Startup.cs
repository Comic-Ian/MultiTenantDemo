using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiTenantDemo.Infrastructure;
using MultiTenantDemo.Model;

namespace MultiTenantDemo
{
    public class Startup
    {
        public ILifetimeScope AutofacContainer { get; private set; }

        // ConfigureServices是注册依赖项的地方,在ConfigureContainer方法之前调用
        public void ConfigureServices(IServiceCollection services)
        {
           // services.AddScoped<TenantInfo>();
            //services.AddScoped<ISqlConnectionResolver, SqlConnectionResolver>();
            services.AddConnectionByDatabase();
            services.AddControllers();
            services.AddOptions();
            services.AddSession();


        }

        // ConfigureContainer是你可以直接注册的地方
        // Autofac在ConfigureServices之后运行
        //这里将override重写在ConfigureServices中进行的注册。
        //不要建造容器;那是工厂替你做的。
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //在这里直接用Autofac注册你自己的东西。不调用build . populate()，这在AutofacServiceProviderFactory中发生。
            //每个依赖一个实例(Instance Per Dependency)(默认)----InstancePerDependency()
            //单一实例(Single Instance) 单例----SingleInstance()
            //每个生命周期作用域一个实例(Instance Per Lifetime Scope)----InstancePerLifetimeScope()
            //每个匹配的生命周期作用域一个实例(Instance Per Matching Lifetime Scope)----InstancePerMatchingLifetimeScope()
            //每个请求一个实例(Instance Per Request) asp.net web请求----InstancePerRequest()
            //每次被拥有一个实例(Instance Per Owned)----InstancePerOwned()
            builder.RegisterType<SqlConnectionResolver>().As<ISqlConnectionResolver>().InstancePerLifetimeScope();
            builder.RegisterType<TenantInfo>().AsSelf().InstancePerLifetimeScope();
        }

        //Configure是添加中间件的地方,在ConfigureContainer之后
        //如果您需要从容器中解决问题,可以使用IApplicationBuilder.ApplicationServices
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<TenantInfoMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
