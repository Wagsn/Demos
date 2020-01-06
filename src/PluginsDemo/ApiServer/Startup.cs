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
        /// 预加载资源路径列表
        /// </summary>
        public IList<string> _presets { get; } = new List<string>();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            var mvcBuilders = services.AddControllers()
                // 启动运行时`Razor`编译，手动添加`View`资源路径
                .AddRazorRuntimeCompilation(options =>
                {
                    foreach (var item in _presets)
                    {
                        options.AdditionalReferencePaths.Add(item);
                    }
                    AdditionalReferencePathHolder.AdditionalReferencePaths = options.AdditionalReferencePaths;
                });

            // 手动配置界面路由映射
            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Add("/Plugins/{2}/Views/{1}/{0}" + Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine.ViewExtension);
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            // 配置触发控制器重新装载
            services.AddSingleton<Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorChangeProvider>(MyActionDescriptorChangeProvider.Instance);
            services.AddSingleton(MyActionDescriptorChangeProvider.Instance);

            // 加载所有已经安装好的插件 TODO 启用禁用 采用一个数据库来保存插件状态数据
            var pluginDir = System.IO.Path.Combine(AppContext.BaseDirectory, "Plugins");
            var pluginFiles = System.IO.Directory.GetFiles(pluginDir, "*.dll");
            // TODO 每个插件分配到一个文件夹中，一个文件夹代表一个插件，里面包含`plugin.json`配置文件，内部`config.json`配置文件，`XxxPlugin.dll`插件主体程序，插件依赖的`Dependents.dll`也需要加载
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
                // 添加插件路由映射
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
