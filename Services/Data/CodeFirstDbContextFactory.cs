using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Net.Service.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Service.Data
{
    
    public class CodeFirstDbContextFactory : IDesignTimeDbContextFactory<CodeFirstDbContext>
    {
        //appsetting.json의 실제 위치
        private const string _configPath = @"C:\Users\allocation\source\repos\Net\Web\appsettings.json";

        public CodeFirstDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CodeFirstDbContext>();
            optionsBuilder.UseSqlServer(new DbConnector(_configPath).GetConnectionString()) ;
            return new CodeFirstDbContext(optionsBuilder.Options);
        }
    }
}
