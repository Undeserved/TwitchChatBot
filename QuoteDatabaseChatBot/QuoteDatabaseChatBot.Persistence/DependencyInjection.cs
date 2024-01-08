using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuoteDatabaseChatBot.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.Persistence {
    public static class DependencyInjection {
        private const string DatabaseName = "QuoteCollection";
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<QuoteCollectionDbContext>(x => x.UseSqlite(configuration.GetConnectionString(DatabaseName)));
            services.AddScoped<IQuoteCollectionDbContext>(x => x.GetService<QuoteCollectionDbContext>());
            return services;
        }
    }
}
