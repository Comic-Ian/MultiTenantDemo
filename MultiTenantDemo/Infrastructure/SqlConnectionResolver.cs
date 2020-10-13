using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MultiTenantDemo.Model;

namespace MultiTenantDemo.Infrastructure
{
    //创建HttpHeaderSqlConnectionResolver并且实现ISqlConnectionResolver接口。这里要做的事情很简单，直接同TenantInfo取值，并且在配置文件查找对应的connectionString。
    public interface ISqlConnectionResolver
    {
        string GetConnection();
    }
    public class SqlConnectionResolver : ISqlConnectionResolver
    {
        private readonly TenantInfo _tenantInfo;
        private readonly IConfiguration _configuration;

        public SqlConnectionResolver(TenantInfo tenantInfo, IConfiguration configuration)
        {
            this._tenantInfo = tenantInfo;
            this._configuration = configuration;
        }

        public string GetConnection()
        {
            var connectionString = _configuration.GetConnectionString(this._tenantInfo.Name);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new NullReferenceException("can not find the connection");
            }

            return connectionString;
        }
    }
}
