using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuoteDatabaseChatBot.Application;
using QuoteDatabaseChatBot.Persistence;
using QuoteDatabaseChatBot.WordCloud.Services;

IHostBuilder hostBuilder = new HostBuilder();
hostBuilder.ConfigureAppConfiguration((hostContext, builder) => {
    builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
});
hostBuilder.ConfigureServices((hostContext, services) => {
    services.AddPersistence(hostContext.Configuration);
    services.AddApplication();
});
IHost host = hostBuilder.Build();

WordCloudService wcs = new WordCloudService(host.Services.GetRequiredService<IMediator>());
wcs.ExportWordCloud(DateTime.Parse("2023-01-01"), DateTime.Parse("2023-12-31"));