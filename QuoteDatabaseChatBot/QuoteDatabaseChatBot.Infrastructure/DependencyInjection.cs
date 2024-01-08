using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuoteDatabaseChatBot.Infrastructure.Settings;

namespace QuoteDatabaseChatBot.Infrastructure {
    public static class DependencyInjection {
        public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration configuration) {
            var _botSettings = configuration.GetSection(typeof(ChatBotSettings).Name);
            services.Configure<ChatBotSettings>(_botSettings);
            return services;
        }
    }
}
