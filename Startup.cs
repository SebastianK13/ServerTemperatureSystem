using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerTemperatureSystem.EFCoreDbContext;
using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Quartz;
using ServerTemperatureSystem.Services.ComponentReadingsProvider;

namespace ServerTemperatureSystem
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) =>
            Configuration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                var jobKey = new JobKey("ReadingsJob");

                q.AddJob<ReadingsJob>( options => 
                    options.WithIdentity(jobKey));

                q.AddTrigger(options => options
                    .ForJob(jobKey)
                    .WithIdentity("ReadingsJob-trigger")
                    .WithCronSchedule("0 0/1 * * * ?"));
            });

            services.AddQuartzHostedService( q =>
                q.WaitForJobsToComplete = true);

            var serverVersion = new MySqlServerVersion(new Version(8,0,25));
            services.AddControllersWithViews();
            services.AddDbContext<AppParamsDbContext>(options => 
            {
                options.UseMySql(Configuration["Data:Readings:ConnectionString"], serverVersion);
                //options.UseLazyLoadingProxies(true);
            });
            services.AddTransient<IReadingsService, ReadingsDataService>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=LoginPage}");
            });
        }
    }
}
