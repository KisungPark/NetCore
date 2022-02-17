using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Service.Config
{
    public class DbConnector
    {
        private readonly string _connectionString = string.Empty;
        public DbConnector(string configPath)
        {
            /*
            - Add NuGet Packages
                (1) Microsoft.Extensions.Configuration
                (2) Microsoft.Extensions.Configuration.Abstractions
                (3) Microsoft.Extensions.Configuration.Json
             */
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(configPath, false);

            _connectionString = configBuilder.Build()["ConnectionStrings:DefaultConnection"];
            //_connectionString = configBuilder.Build().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
