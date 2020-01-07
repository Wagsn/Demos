using ApiServer.AspNetCore.Mvc;
using ApiServer.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务容器插件扩展
    /// </summary>
    public static class IServiceCollectionPluginExtension
    {
        private static IList<string> _presets = new List<string>();

        public static IServiceCollection AddPluginCore(this IServiceCollection services, Action<PluginSetupOptions> configAction)
        {
            var pluginSetupOptions = new PluginSetupOptions();
            configAction(pluginSetupOptions);
            //var config = pluginSetupOptions.Configuration;
            var mvcBuilder = pluginSetupOptions.MvcBuilder;

            // 添加配置
            services.AddOptions();
            // 配置触发控制器重新装载
            services.AddSingleton<IActionDescriptorChangeProvider>(MyActionDescriptorChangeProvider.Instance);
            services.AddSingleton(MyActionDescriptorChangeProvider.Instance);

            // 加载安装好的插件
            var provider = services.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                var razorOptions = scope.ServiceProvider.GetService<MvcRazorRuntimeCompilationOptions>();

                // 加载所有已经安装好的插件 TODO 启用禁用 采用一个数据库来保存插件状态数据
                var pluginDir = System.IO.Path.Combine(AppContext.BaseDirectory, "Plugins");
                if (!System.IO.Directory.Exists(pluginDir))
                {
                    System.IO.Directory.CreateDirectory(pluginDir);
                }
                var pluginFiles = System.IO.Directory.GetFiles(pluginDir, "*.dll");
                // TODO 每个插件分配到一个文件夹中，一个文件夹代表一个插件，里面包含`plugin.json`配置文件，内部`config.json`配置文件，`XxxPlugin.dll`插件主体程序，插件依赖的`Dependents.dll`也需要加载
                foreach (var filePath in pluginFiles)
                {
                    _presets.Add(filePath);
                    var context = new CollectibleAssemblyLoadContext();
                    #region 采用 AssemblyLoadContext.LoadFromAssemblyPath(assemblyPath) 方式加载 Assembly
                    //var assembly = context.LoadFromAssemblyPath(filePath);
                    //var controllerAssemblyPart = new Microsoft.AspNetCore.Mvc.ApplicationParts.AssemblyPart(assembly);
                    //mvcBuilders.PartManager.ApplicationParts.Add(controllerAssemblyPart);
                    #endregion
                    //using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
                    //{
                    using var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
                    var assembly = context.LoadFromStream(fs);
                    var controllerAssemblyPart = new MyAssemblyPart(assembly);
                    mvcBuilder.PartManager.ApplicationParts.Add(controllerAssemblyPart);
                    PluginsLoadContext.Add(System.IO.Path.GetFileNameWithoutExtension(filePath), context);
                    //}
                }
                //
            }

            // 配置`Razor`运行时编译
            mvcBuilder.AddRazorRuntimeCompilation(options =>
            {
                foreach (var item in _presets)
                {
                    options.AdditionalReferencePaths.Add(item);
                }
                AdditionalReferencePathHolder.AdditionalReferencePaths = options.AdditionalReferencePaths;
            });

            // 添加`Razor`页面路由和应用模型约定
            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
            {
                // 2-PluginName, 1-Controller, 0-Action
                options.AreaViewLocationFormats.Add("/Plugins/{2}/Views/{1}/{0}" + Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine.ViewExtension);
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            // 为 IPageConvention 添加委托，以添加应用于 Razor 页面的模型约定。
            //services.Configure<Microsoft.AspNetCore.Mvc.RazorPages.RazorPagesOptions>(options =>
            //{
            //    options.Conventions.Add(new GlobalTemplatePageRouteModelConvention());
            //});

            return services;
        }

        public class PluginSetupOptions
        {
            //public IConfiguration Configuration { get; set; }
            public IMvcBuilder MvcBuilder { get; set; }
        }
    }
}
