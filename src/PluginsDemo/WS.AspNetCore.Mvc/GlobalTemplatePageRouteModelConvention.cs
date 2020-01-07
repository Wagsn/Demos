using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ApiServer.AspNetCore.Mvc
{
    /// <summary>
    /// 只在程序启动时调用(每页面路由对应执行一次apply)
    /// </summary>
    public class GlobalTemplatePageRouteModelConvention : IPageRouteModelConvention
    {
        ///<summary>
        ///运用到所有页面路由模型中，制定页面路由模板,比如访问index页。
        ///路由模板可以是/index 也可以是/index/{可选参数}
        ///</summary>
        ///<param name="model"></param>
        public void Apply(PageRouteModel model)
        {
            var selectorCount = model.Selectors.Count;
            for (var i = 0; i < selectorCount; i++)
            {
                var selector = model.Selectors[i];
                model.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        //执行路由顺序
                        Order = 1,
                        //页面路由模板
                        Template = AttributeRouteModel.CombineTemplates(selector.AttributeRouteModel.Template, "{globalTemplate?}")
                    }
                });
            }
        }
    }
}
