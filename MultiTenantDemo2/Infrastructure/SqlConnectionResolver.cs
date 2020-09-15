using Microsoft.Extensions.Configuration;
using System;

namespace MultiTenantDemo2.Infrastructure
{
    public interface ISqlConnectionResolver
    {
        string GetConnection();

    }

    public class TenantSqlConnectionResolver : ISqlConnectionResolver
    {
        private readonly TenantInfo _tenantInfo;
        private readonly IConfiguration _configuration;
        private readonly ConnectionResolverOption _option;

        public TenantSqlConnectionResolver(TenantInfo tenantInfo, IConfiguration configuration, ConnectionResolverOption option)
        {
            this._option = option;
            this._tenantInfo = tenantInfo;
            this._configuration = configuration;
        }
        public string GetConnection()
        {
            string connectionString = null;
            switch (this._option.Type)
            {
                // 按库分离
                case ConnectionResolverType.ByDatabase:
                    connectionString = _configuration.GetConnectionString(this._tenantInfo.Name);
                    break;
                //按表分离
                case ConnectionResolverType.ByTabel:
                    connectionString = _configuration.GetConnectionString(this._option.ConnectinStringName);
                    break;
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new NullReferenceException("can not find the connection");
            }
            return connectionString;
        }
    }
}
