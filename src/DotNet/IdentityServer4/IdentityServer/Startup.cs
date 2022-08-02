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
                    // QQ������̨���ûص���ַ http://localhost:2692/signin-qq
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

            // �־û� Consent����ǰ�û��Ե�����Ӧ�ö���Դ�����ȷ�ϣ� ������Ӧ�����
            //services.AddScoped<IdentityServer4.Stores.IMessageStore<IdentityServer4.Models.ConsentResponse>>();
            // ApiResourceScopes

            // AddDbContext��AddIdentity����AddIdentityServerǰ����
            // Migrations ָ��˳��
            // add-migration InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
            // add-migration InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
            // add-migration AppDbMigration -c ApplicationDbContext -o .\Data\Migrations\IdentityServer\ApplicationDb
            // update-database -Context ConfigurationDbContext
            // update-database -Context PersistedGrantDbContext
            // update-database -Context ConfigurationDbContext
            // ���ݿ�����ϵͳӦ���û�����������
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // ���� Identity ���� ���ָ�����û��ͽ�ɫ���͵�Ĭ�ϱ�ʶϵͳ����
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
            // ���ݳ־û� https://www.cnblogs.com/laozhang-is-phi/p/10660403.html
            .AddAspNetIdentity<ApplicationUser>()
            // �������ݳ־û����ͻ��� �� ��Դ��
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            // ��Ӳ������� (codes, tokens, consents)
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                // �Զ����� token ����ѡ
                options.EnableTokenCleanup = true;
            })

            // �ڴ�����
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
            // Token����֤�� https://www.cnblogs.com/edisonchou/p/identityserver4_foundation_and_quickstart_01.html
            .AddDeveloperSigningCredential()
            //ʹ�ù̶���֤��,��ֹIdentityServer4��������������������tokenʧЧ
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
        /// ��Ϊ����û��ͨ��UIȥ¼��api,client����Ϣ
        /// ���п�����initһЩĬ����Ϣд�����ݿ�
        /// </summary>
        /// <param name="app"></param>
        public void InitIdentityServerDataBase(IApplicationBuilder app)
        {
            //ApplicationServices���صľ���IServiceProvider������ע�������
            using (var scope = app.ApplicationServices.CreateScope())
            {
                // Update-Database
                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                /*
                 ��������������ôӦ���ֶ�ִ�� Update-Database -Context PersistedGrantDbContext
                 */
                configurationDbContext.Database.Migrate();

                if (!configurationDbContext.Clients.Any())
                {
                    foreach (var client in IdentityConfig.Clients)
                    {
                        //client.ToEntity() ��ѵ�ǰʵ��ӳ�䵽EFʵ��
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
