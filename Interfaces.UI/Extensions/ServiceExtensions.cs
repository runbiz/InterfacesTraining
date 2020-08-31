using Interfaces.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Interfaces.Core.Logging;
using Interfaces.DAL.Contracts;
using Interfaces.DAL.Repositories;
using AutoMapper;
using Interfaces.Core.Mapping;

namespace Interfaces.UI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration Configuration)
        {
            // This sets up the DbContext using the Connection String from appsettings.json
            services.AddDbContext<InterfacesContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
        }

        public static void ConfigureWebServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services
            .AddControllersWithViews()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddRazorPages();
        }

        public static void ConfigureCustomServices(this IServiceCollection services, IConfiguration Configuration)
        {
            // This adds the Logger service that uses NLog to save different log messages
            services.AddSingleton<ILoggerManager, LoggerManager>();

            // This adds our main Repository Wrapper that contains all our Data Access Layer repositories
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }

    }
}
