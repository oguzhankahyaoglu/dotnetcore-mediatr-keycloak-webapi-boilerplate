using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleAPI.API.StartupConfiguration;
using SampleAPI.Data;
using Serilog;

namespace SampleAPI.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Environment = env;
            Configuration = configuration;
        }

        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var businessAssembly = typeof(SampleAPI.Business.MappingProfile).Assembly;
            var contractsAssembly = typeof(SampleAPI.Contracts.Marker).Assembly;

            services.AddDbContext<SampleAPIContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Default"]));
            services.AddAutoMapper(businessAssembly, contractsAssembly);
            services.AddMediatR(businessAssembly, contractsAssembly);
            services
                .AddMvc()
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            SwaggerConfiguration.ConfigureServices(services, Configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            IocConfiguration.RegisterAllDependencies(services);
            AuthConfiguration.ConfigureServices(services, Environment, Configuration);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            SwaggerConfiguration.Configure(app, Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "MyArea",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
