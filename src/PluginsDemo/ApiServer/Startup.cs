using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Logging;

namespace ApiServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Ԥ������Դ·���б�
        /// </summary>
        public IList<string> _presets { get; } = new List<string>();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            var mvcBuilders = services.AddControllers()
                // ��������ʱ`Razor`���룬�ֶ����`View`��Դ·��
                .AddRazorRuntimeCompilation(options =>
                {
                    foreach (var item in _presets)
                    {
                        options.AdditionalReferencePaths.Add(item);
                    }
                    AdditionalReferencePathHolder.AdditionalReferencePaths = options.AdditionalReferencePaths;
                });

            // �ֶ����ý���·��ӳ��
            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Add("/Plugins/{2}/Views/{1}/{0}" + Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine.ViewExtension);
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            // ���ô�������������װ��
            services.AddSingleton<Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorChangeProvider>(MyActionDescriptorChangeProvider.Instance);
            services.AddSingleton(MyActionDescriptorChangeProvider.Instance);

            // ���������Ѿ���װ�õĲ�� TODO ���ý��� ����һ�����ݿ���������״̬����
            var pluginDir = System.IO.Path.Combine(AppContext.BaseDirectory, "Plugins");
            var pluginFiles = System.IO.Directory.GetFiles(pluginDir, "*.dll");
            // TODO ÿ��������䵽һ���ļ����У�һ���ļ��д���һ��������������`plugin.json`�����ļ����ڲ�`config.json`�����ļ���`XxxPlugin.dll`���������򣬲��������`Dependents.dll`Ҳ��Ҫ����
            foreach (var filePath in pluginFiles)
            {
                _presets.Add(filePath);
                var context = new CollectibleAssemblyLoadContext();
                //var assembly = context.LoadFromAssemblyPath(filePath);
                //var controllerAssemblyPart = new Microsoft.AspNetCore.Mvc.ApplicationParts.AssemblyPart(assembly);
                //mvcBuilders.PartManager.ApplicationParts.Add(controllerAssemblyPart);
                using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
                {
                    var assembly = context.LoadFromStream(fs);

                    var controllerAssemblyPart = new MyAssemblyPart(assembly);

                    mvcBuilders.PartManager.ApplicationParts.Add(controllerAssemblyPart);
                    PluginsLoadContext.AddPluginContext(System.IO.Path.GetFileNameWithoutExtension(filePath), context);
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // ��Ӳ��·��ӳ��
                endpoints.MapControllerRoute(
                   name: "Customer",
                   pattern: "Modules/{area}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class MyAssemblyPart : Microsoft.AspNetCore.Mvc.ApplicationParts.AssemblyPart, Microsoft.AspNetCore.Mvc.ApplicationParts.ICompilationReferencesProvider
    {
        public MyAssemblyPart(System.Reflection.Assembly assembly) : base(assembly) { }

        public IEnumerable<string> GetReferencePaths() => Array.Empty<string>();
    }

    public static class AdditionalReferencePathHolder
    {
        public static IList<string> AdditionalReferencePaths = new List<string>();
    }
}
