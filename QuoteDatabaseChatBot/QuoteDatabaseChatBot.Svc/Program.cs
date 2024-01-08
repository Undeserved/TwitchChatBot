using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuoteDatabaseChatBot.Application;
using QuoteDatabaseChatBot.Infrastructure;
using QuoteDatabaseChatBot.Persistence;
using QuoteDatabaseChatBot.Svc;
using QuoteDatabaseChatBot.Svc.Services;

IHostBuilder hostBuilder = new HostBuilder();
hostBuilder.ConfigureAppConfiguration((hostContext, builder) => {
    builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
});
hostBuilder.ConfigureServices((hostContext, services) => {
    services.AddInfrastructure(hostContext.Configuration);
    services.AddPersistence(hostContext.Configuration);
    services.AddApplication();
    services.AddChatBotServices(hostContext.Configuration);
});
IHost host = hostBuilder.Build();

TwitchChatService _chatservice = host.Services.GetRequiredService<TwitchChatService>();

Console.ReadLine();
