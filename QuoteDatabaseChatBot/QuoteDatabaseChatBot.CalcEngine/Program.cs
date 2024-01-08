using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuoteDatabaseChatBot.Application;
using QuoteDatabaseChatBot.Persistence;
using QuoteDatabaseChatBot.CalcEngine.Services;

IHostBuilder hostBuilder = new HostBuilder();
hostBuilder.ConfigureAppConfiguration((hostContext, builder) => {
    builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
});
hostBuilder.ConfigureServices((hostContext, services) => {
    services.AddPersistence(hostContext.Configuration);
    services.AddApplication();
});
IHost host = hostBuilder.Build();