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
            var mvcBuilder = services.AddControllers();

            // ��Ӳ������
            services.AddPlugins(options =>
            {
                options.MvcBuilder = mvcBuilder;
                //options.Configuration = Configuration;
            });
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
}
