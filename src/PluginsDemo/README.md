# Plugins Demo

参考
- https://blog.csdn.net/weixin_33898876/article/details/94100934

- [从零开始实现ASP.NET Core MVC的插件式开发 系列文章](https://www.cnblogs.com/lwqlun/p/11137788.html#1437645483)
	- 使用Application Part动态加载控制器和视图 基于`ApplicationPartManager`
	- 如何创建项目模板 基于`vs`项目模板生成工具
	- 如何在运行时启用组件 基于自定义`IActionDescriptorChangeProvider`
	- 插件安装 公共抽象接口约束`IPlugin`以及插件配置文档定义`plugin.json`以及`ZipTool`解压工具
	- 插件的删除和升级 基于`.NET Core 3.0`的`AssemblyLoadContext` [如何在 .NET Core 中使用和调试程序集可卸载性](https://docs.microsoft.com/zh-cn/dotnet/standard/assembly/unloadability?view=netcore-2.2)

两种插件上传策略
- HTTP请求的方式加载插件
- 通过文件系统的方式加载插件
	- [文件系统的监控](https://www.cnblogs.com/artech/p/net-core-file-provider-01.html)
