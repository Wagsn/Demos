using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.WebApi
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
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityServer.ApiServer", Version = "v1" });

                // https://stackoverflow.com/questions/43447688/setting-up-swagger-asp-net-core-using-the-authorization-headers-bearer
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme,
                            }
                        }, new List<string>()
                    }
                });

                // add auth header for [Authorize] endpoints
                c.OperationFilter<AddAuthHeaderOperationFilter>();
                c.OperationFilter<CustomHeaderSwaggerAttribute>();
            });

            // this API will accept any access token from the authority
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    // base-address of your identityserver
                    options.Authority = "https://localhost:44370";

                    // if you are using API resources, you can specify the name here
                    options.TokenValidationParameters.ValidateAudience = false;
                    //options.Audience = "weatherApi";

                    options.TokenValidationParameters.ValidateIssuer = false;
                    //options.TokenValidationParameters.ValidIssuer = "https://localhost:44370";

                    // IdentityServer emits a typ header by default, recommended extra check
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

                    // if token does not contain a dot, it is a reference token
                    options.ForwardDefaultSelector = context =>
                    {
                        if (context.Request.Headers.TryGetValue("Authorization", out var authValues))
                        {
                            if (!authValues.First().Contains("."))
                            {
                                return "introspection";
                            }
                        }
                        return JwtBearerDefaults.AuthenticationScheme;
                    };

                    options.SaveToken = true;
                })
                .AddOAuth2Introspection("introspection", options =>
                {
                    options.Authority = "https://localhost:44370";

                    // this maps to the API resource name and secret
                    options.ClientId = "weatherApi";
                    options.ClientSecret = IdentityServer4.Models.HashExtensions.Sha256("ProCodeGuide");
                })
                ;

            services.AddAuthorization(options =>
            {
                /*// check scope 
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "weatherApi.read");
                });*/
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer.WebApi v1"));
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseCors(policy =>
            {
                // Allow cors origins
                policy.WithOrigins("https://localhost:44370");

                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithExposedHeaders("WWW-Authenticate");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    // check scope
                    .RequireAuthorization("weatherApiScope")
                    // check any request
                    //.RequireAuthorization()
                    ;
            });
        }
    }
}
