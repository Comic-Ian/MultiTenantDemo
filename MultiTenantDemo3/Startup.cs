using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiTenantDemo3.Infrastructure;
using MultiTenantDemo3.Model;

namespace MultiTenantDemo3
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            TestOne();
            TestTwo();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }

        static void TestOne()
        {
            TestContext context = new TestContext();
            int count = context.People.Count();
            Console.WriteLine(count);
        }
        static void TestTwo()
        {
            DateTime start = DateTime.Now;
            TestContext context = new TestContext();
            for (int i = 0; i < 10000; i++)
            {
                context.People.Add(new People()
                {
                    Name = "a"+i,
                    Sex = false,
                    BirNum = i
                });
                context.SaveChanges();
            }
            Console.WriteLine(context.People.Count());
            Console.WriteLine("总时间,秒数：" + (DateTime.Now - start).TotalSeconds);
        }
    }
}
