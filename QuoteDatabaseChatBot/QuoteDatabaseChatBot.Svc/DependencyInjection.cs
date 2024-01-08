using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QuoteDatabaseChatBot.Infrastructure.Settings;
using QuoteDatabaseChatBot.Svc.Services;
using QuoteDatabaseChatBot.Svc.Services.ChatCommands;
using QuoteDatabaseChatBot.Svc.Services.MessageParsers;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace QuoteDatabaseChatBot.Svc {
    internal static class DependencyInjection {
        public static IServiceCollection AddChatBotServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddSingleton< ITwitchClient, TwitchClient>(x => {
                ChatBotSettings chatBotSettings = x.GetRequiredService<IOptions<ChatBotSettings>>().Value;
                ConnectionCredentials credentials = new ConnectionCredentials(chatBotSettings.TwitchAccount, chatBotSettings.OAuthToken);
                ClientOptions clientOptions = new ClientOptions {
                    MessagesAllowedInPeriod = 600,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };
                WebSocketClient customClient = new WebSocketClient(clientOptions);
                TwitchClient twitchClient = new TwitchClient(customClient);
                twitchClient.Initialize(credentials, channel: chatBotSettings.TargetChannel);
                return twitchClient;
            });
            services.AddSingleton<ITwitchAPI, TwitchAPI>(x => {
                ChatBotSettings chatBotSettings = x.GetRequiredService<IOptions<ChatBotSettings>>().Value;
                TwitchAPI api = new TwitchAPI();
                api.Settings.ClientId = chatBotSettings.ClientId;
                api.Settings.AccessToken = chatBotSettings.OAuthToken;
                return api;

            });
            services.AddSingleton<TwitchChatService>();
            var commands = typeof(IChatCommand)
                .Assembly.GetTypes()
                .Where(x => !x.IsAbstract && 
                            x.IsClass &&
                            x.GetInterface(nameof(IChatCommand)) == typeof(IChatCommand));

            foreach (var item in commands) {
                services.Add(new ServiceDescriptor(typeof(IChatCommand), item, ServiceLifetime.Scoped));
            }

            var messageParsers = typeof(IMessageParser)
                .Assembly.GetTypes()
                .Where(x => !x.IsAbstract &&
                            x.IsClass &&
                            x.GetInterface(nameof(IMessageParser)) == typeof(IMessageParser));
            foreach (var item in messageParsers) {
                services.Add(new ServiceDescriptor(typeof(IMessageParser), item, ServiceLifetime.Scoped));
            }

            return services;
        }
    }
}
