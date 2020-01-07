using ApiServer.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    /// <summary>
    /// 插件管理器
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PluginController : Controller
    {
        public PluginController(ApplicationPartManager applicationPartManager) 
        {
            _partManager = applicationPartManager;
        }
        ApplicationPartManager _partManager { get; }
        //private IReferenceLoader _referenceLoader { get; }
        /// <summary>
        /// 删除插件
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public void DeletePlugin(string pluginName)
        {
            var last = _partManager.ApplicationParts.First(p => p.Name == pluginName);
            _partManager.ApplicationParts.Remove(last);

            ResetControllActions();
        }

        //public void EnableModule(string moduleName)

        //public void DisableModule(string moduleName)

        /// <summary>
        /// 重新装载控制器
        /// </summary>
        private void ResetControllActions()
        {
            MyActionDescriptorChangeProvider.Instance.HasChanged = true;
            MyActionDescriptorChangeProvider.Instance.TokenSource.Cancel();
        }
    }
}
