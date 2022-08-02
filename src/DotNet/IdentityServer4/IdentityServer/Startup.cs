using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
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
            services.AddAuthentication()
                // Unicorn.AspNetCore.Authentication.QQ
                /*.AddQQ("", options=>
                {
                    // QQ互联后台配置回调地址 http://localhost:2692/signin-qq
                    options.AppId = "<insert here>";
                    options.AppKey = "<insert here>";
                })*/
                /*.AddGoogle(options =>
                {
                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                })*/
                ;

            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;

            //services.AddConfigurationDbContext<>();
            //services.AddOperationalDbContext<>();

            // 持久化 Consent（当前用户对第三方应用对资源请求的确认） 第三方应用许可
            //services.AddScoped<IdentityServer4.Stores.IMessageStore<IdentityServer4.Models.ConsentResponse>>();
            // ApiResourceScopes

            // AddDbContext和AddIdentity需在AddIdentityServer前调用
            // Migrations 指令顺序
            // add-migration InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
            // add-migration InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
            // add-migration AppDbMigration -c ApplicationDbContext -o .\Data\Migrations\IdentityServer\ApplicationDb
            // update-database -Context ConfigurationDbContext
            // update-database -Context PersistedGrantDbContext
            // update-database -Context ConfigurationDbContext
            // 数据库配置系统应用用户数据上下文
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // 启用 Identity 服务 添加指定的用户和角色类型的默认标识系统配置
            //services.AddDefaultIdentity<ApplicationUser>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            // 数据持久化 https://www.cnblogs.com/laozhang-is-phi/p/10660403.html
            .AddAspNetIdentity<ApplicationUser>()
            // 配置数据持久化（客户端 和 资源）
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            // 添加操作数据 (codes, tokens, consents)
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                // 自动清理 token ，可选
                options.EnableTokenCleanup = true;
            })

            // 内存数据
            .AddInMemoryClients(IdentityConfig.Clients)
            //.AddClientStore<>()
            .AddInMemoryIdentityResources(IdentityConfig.IdentityResources)
            .AddInMemoryApiResources(IdentityConfig.ApiResources)
            .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
            .AddInMemoryCaching()
            //.AddAuthorizationParametersMessageStore<>()
            //.AddResourceOwnerValidator<RoleTestResourceOwnerPasswordValidator>()
            //.AddExtensionGrantValidator<>()
            //.AddProfileService<>()
            .AddTestUsers(IdentityConfig.TestUsers)
            // Token加密证书 https://www.cnblogs.com/edisonchou/p/identityserver4_foundation_and_quickstart_01.html
            .AddDeveloperSigningCredential()
            //使用固定的证书,防止IdentityServer4服务器重启导致以往的token失效
            //AddDeveloperSigningCredential(true, "tempkey.jwk");
            ;

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitIdentityServerDataBase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();


            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }

        /// <summary>
        /// 因为现在没有通过UI去录入api,client等信息
        /// 所有可以先init一些默认信息写入数据库
        /// </summary>
        /// <param name="app"></param>
        public void InitIdentityServerDataBase(IApplicationBuilder app)
        {
            //ApplicationServices返回的就是IServiceProvider，依赖注入的容器
            using (var scope = app.ApplicationServices.CreateScope())
            {
                // Update-Database
                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                /*
                 如果不走这个，那么应该手动执行 Update-Database -Context PersistedGrantDbContext
                 */
                configurationDbContext.Database.Migrate();

                if (!configurationDbContext.Clients.Any())
                {
                    foreach (var client in IdentityConfig.Clients)
                    {
                        //client.ToEntity() 会把当前实体映射到EF实体
                        configurationDbContext.Clients.Add(client.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
                if (!configurationDbContext.ApiResources.Any())
                {
                    foreach (var api in IdentityConfig.ApiResources)
                    {
                        configurationDbContext.ApiResources.Add(api.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
                if (!configurationDbContext.IdentityResources.Any())
                {
                    foreach (var identity in IdentityConfig.IdentityResources)
                    {
                        configurationDbContext.IdentityResources.Add(identity.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
                if (!configurationDbContext.ApiScopes.Any())
                {
                    foreach (var identity in IdentityConfig.ApiScopes)
                    {
                        configurationDbContext.ApiScopes.Add(identity.ToEntity());
                    }
                    configurationDbContext.SaveChanges();
                }
                if (configurationDbContext.ClientCorsOrigins.Any())
                {
                    //
                }

                var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                applicationDbContext.Database.Migrate();
                if (!applicationDbContext.Users.Any())
                {
                    applicationDbContext.SaveChanges();
                }
            }
        }
    }
}
