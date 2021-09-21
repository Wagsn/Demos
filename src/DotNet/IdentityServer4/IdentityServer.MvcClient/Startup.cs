using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.MvcClient
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
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            // authorization_code & form_post
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:44370";
                // 使用非HTTPS的Authority
                //options.RequireHttpsMetadata = false;
                options.ClientId = "oidcMVCApp";
                options.ClientSecret = "ProCodeGuide";

                //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.UsePkce = true;
                // default: form_post
                //options.ResponseMode = "query";

                // Invalid grant type for client: hybrid
                //options.ResponseType = "code id_token";
                // authorization_code 
                options.ResponseType = "code";
                // Requests for id_token response type only must not include resource scopes
                options.Scope.Add("weatherApi.read");
                // 如果带了其他scope时想要返回refresh_token时则带上offline_access
                options.Scope.Add("offline_access");
                options.SaveTokens = true;
            })
            // OpenID Connect implicit flow client (MVC)  采用隐式认证的MVC客户端
            /*.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Authority = "https://localhost:44370";
                options.RequireHttpsMetadata = false;

                options.ClientId = "mvc";
                options.SaveTokens = true;
            })*/
            // HybridAndClientCredentials  code_challenge is missing
            /*.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Authority = "https://localhost:44370";
                //options.RequireHttpsMetadata = false;

                options.ClientId = "mvc.hybrid";
                options.ClientSecret = "secret";
                options.ResponseType = "code id_token";

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add("weatherApi.read");
                // 为了请求一个刷新令牌，客户端需要在请求中包含一个offline_access的scope（并且这个scope必须通过认证）
                options.Scope.Add("offline_access");
                options.ClaimActions.MapJsonKey("website", "website");

                options.UsePkce = true;
            })*/
            ;

            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
