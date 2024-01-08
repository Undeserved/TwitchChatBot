using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Persistence {
    public abstract class DesignTimeDbContextFactoryBase<TContext>
        : IDesignTimeDbContextFactory<TContext> where TContext : DbContext {
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";
        private const string ConnectionStringName = "QuoteCollection";

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        public TContext CreateDbContext(string[] args) {
            var basePath = Directory.GetCurrentDirectory() + String.Format("{0}..{0}QuoteCollectionContext", Path.DirectorySeparatorChar);
            return Create(basePath, Environment.GetEnvironmentVariable(AspNetCoreEnvironment));
        }

        private TContext Create(string basePath, string environmentName) {
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            string connectionString = config.GetConnectionString(ConnectionStringName);
            return Create(connectionString);
        }

        private TContext Create(string databaseName) {
            if (string.IsNullOrEmpty(databaseName)) {
                throw new ArgumentException($"Couldn't find '{ConnectionStringName}' in the ConnectionStrings section of appsettings.json.", nameof(databaseName));
            }

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseSqlite(databaseName);

            return CreateNewInstance(optionsBuilder.Options);
        }
    }
}
