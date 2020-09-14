using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MultiTenantDemo.Model
{
    public class TenantInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tenantInfo = context.RequestServices.GetRequiredService<TenantInfo>();
            var tenantName = context.Request.Headers["Tenant"];

            if(string.IsNullOrEmpty(tenantName))
            {
                tenantName = "default";
            }

            tenantInfo.Name = tenantName;

            await _next(context);
        }
    }
}
