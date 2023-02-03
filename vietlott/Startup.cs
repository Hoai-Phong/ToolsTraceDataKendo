using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vietlott.Services;
using vietlott.Settings;

namespace vietlott
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }
        public Startup()
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<VietlottContext>(
                options => options.UseSqlServer(
                Configuration.GetConnectionString("VietlottConnection"))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()

            );

            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<IToolConfiguration, ToolConfiguration>();
            services.AddHttpClient();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddNLog("nlog.config");
            });

            services.AddTransient<KendoServices>();
        }
    }
}
