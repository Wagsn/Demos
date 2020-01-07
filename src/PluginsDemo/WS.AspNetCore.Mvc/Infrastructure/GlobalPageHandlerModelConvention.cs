using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.AspNetCore.Mvc.Infrastructure
{
    /// <summary>
    /// 页面处理程序模型构造过程中应用。
    /// </summary>
    public class GlobalPageHandlerModelConvention : IPageHandlerModelConvention
    {
        //页面加载时调用(每一个路由地址)，在IPageApplicationModelConvention约定之后执行
        public void Apply(PageHandlerModel model)
        {
            //目前还不清楚能做什么
        }
    }
}
