using Microsoft.Extensions.Configuration;
using System.IO;

namespace MultiTenantDemo3.Infrastructure
{
    public class GetConnection
    {
        public readonly static IConfiguration Configuration;
        static GetConnection()
        {
            //
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

        public static string Conn
        {
            get { return Configuration.GetConnectionString("Test"); }
        }
    }
}
