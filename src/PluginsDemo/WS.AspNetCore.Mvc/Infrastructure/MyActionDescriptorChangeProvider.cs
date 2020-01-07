namespace ApiServer.AspNetCore.Mvc.Infrastructure
{
    /// <summary>
    /// 触发控制器重新装载
    /// 需要在`Startup.cs`的`ConfigureServices`方法中，将`MyActionDescriptorChangeProvider.Instance`属性以单例的方式注册到依赖注入容器中
    /// </summary>
    public class MyActionDescriptorChangeProvider : Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorChangeProvider
    {
        public static MyActionDescriptorChangeProvider Instance { get; } = new MyActionDescriptorChangeProvider();

        public System.Threading.CancellationTokenSource TokenSource { get; private set; }

        public bool HasChanged { get; set; }

        public Microsoft.Extensions.Primitives.IChangeToken GetChangeToken()
        {
            TokenSource = new System.Threading.CancellationTokenSource();
            return new Microsoft.Extensions.Primitives.CancellationChangeToken(TokenSource.Token);
        }
    }
}
