using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp_TcpServer_Console.Data
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        private static IConfigurationRoot config;
        public ApplicationContext CreateDbContext(string[] args)
        {
            if (!File.Exists("AppSettings.json"))
            {
                throw new FileNotFoundException("Configuration file 'AppSettings.json' not found.");
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json");

            config = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}