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

        // ConfigureServices��ע��������ĵط�,��ConfigureContainer����֮ǰ����
        public void ConfigureServices(IServiceCollection services)
        {
           // services.AddScoped<TenantInfo>();
            //services.AddScoped<ISqlConnectionResolver, SqlConnectionResolver>();
            services.AddConnectionByDatabase();
            services.AddControllers();
            services.AddOptions();
            services.AddSession();


        }

        // ConfigureContainer�������ֱ��ע��ĵط�
        // Autofac��ConfigureServices֮������
        //���ｫoverride��д��ConfigureServices�н��е�ע�ᡣ
        //��Ҫ��������;���ǹ����������ġ�
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //������ֱ����Autofacע�����Լ��Ķ�����������build . populate()������AutofacServiceProviderFactory�з�����
            //ÿ������һ��ʵ��(Instance Per Dependency)(Ĭ��)----InstancePerDependency()
            //��һʵ��(Single Instance) ����----SingleInstance()
            //ÿ����������������һ��ʵ��(Instance Per Lifetime Scope)----InstancePerLifetimeScope()
            //ÿ��ƥ�����������������һ��ʵ��(Instance Per Matching Lifetime Scope)----InstancePerMatchingLifetimeScope()
            //ÿ������һ��ʵ��(Instance Per Request) asp.net web����----InstancePerRequest()
            //ÿ�α�ӵ��һ��ʵ��(Instance Per Owned)----InstancePerOwned()
            builder.RegisterType<SqlConnectionResolver>().As<ISqlConnectionResolver>().InstancePerLifetimeScope();
            builder.RegisterType<TenantInfo>().AsSelf().InstancePerLifetimeScope();
        }

        //Configure������м���ĵط�,��ConfigureContainer֮��
        //�������Ҫ�������н������,����ʹ��IApplicationBuilder.ApplicationServices
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
