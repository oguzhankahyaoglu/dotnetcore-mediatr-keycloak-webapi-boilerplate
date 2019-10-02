using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleAPI.API.StartupConfiguration
{
    public static class SwaggerConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.SwaggerDoc("v1", new Info {Title = "SampleAPI", Version = "v1"});
                c.DescribeAllEnumsAsStrings();
                // JWT-token authentication by password
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OAuth2Scheme
                {
                    Flow =  Configuration["Jwt:Flow"],
                    TokenUrl = Configuration["Jwt:TokenUrl"],
                    AuthorizationUrl = Configuration["Jwt:AuthUrl"]
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {JwtBearerDefaults.AuthenticationScheme, new string[0]}
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }


        public static void Configure(IApplicationBuilder app, IConfiguration Configuration)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleAPI");
                // Swashbuckle.AspNetCore 4.0.1
                c.OAuthClientId(Configuration["Jwt:ClientId"]);
//                c.OAuthClientSecret(Configuration["Jwt:ClientSecret"]);
                c.OAuthRealm(Configuration["Jwt:Realm"]);
            });
        }

        public class AuthorizeCheckOperationFilter : IOperationFilter
        {
            public void Apply(Operation operation, OperationFilterContext context)
            {
                var hasAuthorize =
                    context.ApiDescription.ActionAttributes()
                        .OfType<AuthorizeAttribute>()
                        .Any();

                hasAuthorize = hasAuthorize ||
                               context.ApiDescription.ControllerAttributes()
                                   .OfType<AuthorizeAttribute>()
                                   .Any();

                if (hasAuthorize)
                {
                    operation.Responses.Add("401", new Response {Description = "Unauthorized"});
                    operation.Responses.Add("403", new Response {Description = "Forbidden"});

                    operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                    {
                        new Dictionary<string, IEnumerable<string>> {{JwtBearerDefaults.AuthenticationScheme, new string[0] { }}}
                    };
                }
            }
        }
    }
}
