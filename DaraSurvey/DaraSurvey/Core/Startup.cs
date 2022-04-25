using AutoMapper;
using DaraSurvey.Core.PackagesConfig;
using DaraSurvey.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DaraSurvey.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // --------------------

        public IConfiguration Configuration { get; }
        public static IServiceCollection ServiceCollection { get; set; }

        // --------------------

        public void ConfigureServices(IServiceCollection services)
        {
            ServiceCollection = services;
            var appSettings = services.GetAppSettings(Configuration);

            services.AllowSynchronousIO();
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddMvc(options => { options.Filters.Add(new ModelStateValidator()); });
            services.AddAutoMapper(typeof(Startup));
            services.Configure<AppSettings>(options => Configuration.Bind(options));
            services.AddSqlServerDatabase(Configuration);
            services.IdentityConfig();
            services.JwtConfig(appSettings);
            services.AddCorsConfigs();
            services.AddControllers();
            services.AddContainers();
            services.SwaggerConfig();
        }

        // --------------------

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            app.EnsureMigrationOfContext<DB>();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSwagerDocumentation();
            app.UseCors("EnableCors");
            app.UseCustomExceptionHandler(env);
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            IdentityConfiguration.IdentitySeedAsync(userManager, roleManager).Wait();
        }
    }
}