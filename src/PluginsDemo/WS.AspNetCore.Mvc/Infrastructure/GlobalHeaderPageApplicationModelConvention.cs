using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.AspNetCore.Mvc.Infrastructure
{
    ///<summary>
    /// 页面加载时调用(每一个路由地址)
    ///</summary>
    public class GlobalHeaderPageApplicationModelConvention : IPageApplicationModelConvention
    {
        public void Apply(PageApplicationModel model)
        {
            model.Filters.Add(new AddHeaderAttribute(
            "GlobalHeader", new string[] { "Global Header Value" }));
        }
    }

    public class AddHeaderAttribute : ResultFilterAttribute
    {
        private readonly string _name;
        private readonly string[] _values;

        public AddHeaderAttribute(string name, string[] values)
        {
            _name = name;
            _values = values;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add(_name, _values);
            base.OnResultExecuting(context);
        }
    }
}
