using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuoteDatabaseChatBot.CalcEngine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.CalcEngine {
    internal static class DependencyInjection {
        public static IServiceCollection ConfigureReportSettings(this IServiceCollection services, IConfiguration configuration) {
            var _botSettings = configuration.GetSection(typeof(ReportSettings).Name);
            services.Configure<ReportSettings>(_botSettings);
            return services;
        }
    }
}
