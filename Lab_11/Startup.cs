﻿using Lab_11.Filters;

namespace Lab_11
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.txt");

            services.AddScoped<LoggingFilter>(provider => new LoggingFilter(logFilePath));
            services.AddScoped<UniqueUserFilter>(provider => new UniqueUserFilter(logFilePath));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "main",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}