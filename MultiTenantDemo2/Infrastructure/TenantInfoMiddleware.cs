using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MultiTenantDemo2.Infrastructure
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

            if (string.IsNullOrEmpty(tenantName))
                tenantName = "default";

            tenantInfo.Name = tenantName;

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
