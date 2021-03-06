using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Interfaces.Core.Logging;
using Interfaces.Core.Mapping;
using Interfaces.DAL.Contracts;
using Interfaces.DAL.Repositories;
using Interfaces.UI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NLog;

namespace Interfaces.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // This is used by the NLog package to check it's config file to determine the location to save the log file
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /********************** Configured in Extensions\ServiceExtensions.cs **********************/

            // Configures the DbContext and ties it to the connection string in appsettings.json
            services.ConfigureDbContext(Configuration);

            // Adds any custom-built services to the application (ex. ILoggerManager, IRepositoryBase)
            services.ConfigureCustomServices(Configuration);

            // Adds AutoMapper to the application and binds our MappingProfile class to it 
            services.ConfigureAutoMapper(Configuration);

            // Adds Razor Pages, Controllers, and any other web-based services to the application
            services.ConfigureWebServices(Configuration);
            
            /******************************************************************************************/

            services.AddKendo();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Pages")),
                RequestPath = "/Pages"
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
