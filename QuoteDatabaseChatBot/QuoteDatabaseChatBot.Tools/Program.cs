using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuoteDatabaseChatBot.Application;
using QuoteDatabaseChatBot.Persistence;

namespace QuoteDatabaseChatBot.Tools {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            IHostBuilder hostBuilder = new HostBuilder();
            hostBuilder.ConfigureAppConfiguration((hostContext, builder) => {
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            });
            hostBuilder.ConfigureServices((hostContext, services) => {
                services.AddScoped<ToolsForm>();
                services.AddPersistence(hostContext.Configuration);
                services.AddApplication();
            });
            IHost host = hostBuilder.Build();
            var mainForm = host.Services.GetRequiredService<ToolsForm>();
            System.Windows.Forms.Application.Run(mainForm);
        }
    }
}