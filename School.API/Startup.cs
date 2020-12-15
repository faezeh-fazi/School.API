using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using School.API.Installer;
using School.DataAccess;
using School.Helpers;
using School.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace School.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.InstallServicesAssembly(Configuration);
            services.AddAutoMapper(typeof(Startup));



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DiscDbContext context, UserManager<User> manager, RoleManager<IdentityRole> roleManager)
             
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            var swaggerSettings = new SwaggerSettings();
            Configuration.GetSection(nameof(swaggerSettings)).Bind(swaggerSettings);
            app.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerSettings.JsonRoute;
            });
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(swaggerSettings.UiEndPoint, swaggerSettings.Description);
                setupAction.DocExpansion(DocExpansion.None);
            });



            app.UseHttpsRedirection();

            //UseStaticFiles();
            //UseDefaultFiles();
            app.UseRouting();
            app.UseCors("DiscPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            DbInitializer.SeedData(context, manager, roleManager);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
