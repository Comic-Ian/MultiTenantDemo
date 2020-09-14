using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiTenantDemo
{
    //创建HttpHeaderSqlConnectionResolver并且实现ISqlConnectionResolver接口。这里要做的事情很简单，直接同TenantInfo取值，并且在配置文件查找对应的connectionString。
    public interface ISqlConnectionResolver
    { 
        string GetConnection();
    }
    public class HttpHeaderSqlConnectionResolver : ISqlConnectionResolver
    {

        public string GetConnection()
        {
            throw new NotImplementedException();
        }
    }
}
