using GitServer.Services;
using GitServer.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GitServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using GitServer.Handlers;
using GitServer.ApplicationCore.Interfaces;
using GitServer.Infrastructure.Repositories;
using GitServer.ApplicationCore.Models;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace GitServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionType = Configuration.GetConnectionString("ConnectionType");
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            switch (connectionType)
            {
                case "Sqlite":
                    services.AddDbContext<GitServerContext>(options => options.UseSqlite(connectionString));
                    break;
                case "MSSQL":
                    services.AddDbContext<GitServerContext>(options => options.UseSqlServer(connectionString));
                    break;
                case "MySQL":
                    services.AddDbContext<GitServerContext>(options => options.UseMySQL(connectionString));
                    break;
                default:
                    services.AddDbContext<GitServerContext>(options => options.UseSqlite(connectionString));
                    break;
            }

            // Add framework services.
            services.AddMvc(option => {
                option.EnableEndpointRouting = false;
            });

            services.AddOptions();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options => {
                options.AccessDeniedPath = "/User/Login";
                options.LoginPath = "/User/Login";
            }).AddBasic();

            // Add settings
            services.Configure<GitSettings>(Configuration.GetSection(nameof(GitSettings)));
            // Add git services
            services.AddTransient<GitRepositoryService>();
            services.AddTransient<GitFileService>();
            services.AddTransient<IRepository<User>, Repository<User>>();
            services.AddTransient<IRepository<Repository>, Repository<Repository>>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                     new CultureInfo("en"),
                     new CultureInfo("ru"),
                     new CultureInfo("uk")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: Constants.DefaultCulture, uiCulture: Constants.DefaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeGitServerDatabase(app.ApplicationServices);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc(routes => RouteConfig.RegisterRoutes(routes));

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

        }

        private void InitializeGitServerDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<GitServerContext>();
                db.Database.EnsureCreated();
                if (db.Users.Count() == 0)
                {
                    //db.SaveChanges();
                }
            }
        }
    }
}
