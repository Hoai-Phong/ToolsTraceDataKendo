using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using Vietlott.Services.Settings;
using Vietlott.Services;
using Vietlott.DataAccess;

namespace Vietlott.Services
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
                options => options.UseSqlServer(Configuration.GetConnectionString("VietlottConnection"))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()

            );

            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<ToolConfiguration>();
            services.AddHttpClient();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddNLog("nlog.config");
            });

            services.AddTransient<KendoServices>();
        }
    }
}
