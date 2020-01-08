# Plugins Demo

���ʵ�ֻ��� ApplicationPartManager+IActionDescriptorChangeProvider+AssemblyLoadContext+�ӿ�Լ��

�ο�
- https://blog.csdn.net/weixin_33898876/article/details/94100934

- [���㿪ʼʵ��ASP.NET Core MVC�Ĳ��ʽ���� ϵ������](https://www.cnblogs.com/lwqlun/p/11137788.html#1437645483)
	- ʹ��Application Part��̬���ؿ���������ͼ ����`ApplicationPartManager`
	- ��δ�����Ŀģ�� ����`vs`��Ŀģ�����ɹ���
	- ���������ʱ������� �����Զ���`IActionDescriptorChangeProvider`
	- �����װ ��������ӿ�Լ��`IPlugin`�Լ���������ĵ�����`plugin.json`�Լ�`ZipTool`��ѹ����
	- �����ɾ�������� ����`.NET Core 3.0`��`AssemblyLoadContext` [����� .NET Core ��ʹ�ú͵��Գ��򼯿�ж����](https://docs.microsoft.com/zh-cn/dotnet/standard/assembly/unloadability?view=netcore-2.2)

���ֲ���ϴ�����
- HTTP����ķ�ʽ���ز��
- ͨ���ļ�ϵͳ�ķ�ʽ���ز��
	- [�ļ�ϵͳ�ļ��](https://www.cnblogs.com/artech/p/net-core-file-provider-01.html)

���ϵͳ�����
- ����ĳ���
- ����Ŀ���
- ����İ�װ
	- ���ļ�ϵͳ������������
- ���������
- ����Ľ���
- �����ж��

��Ŀ����ṹ
```
- ApiServer.dll
- ApiServer.exe
- ApiServer.pdb
- PluginCore.dll
- PluginCore.pdb
- PluginSDK.dll
- Plugins
	- <Guid>
		- plugin.json
		- config.json
		- <PluginDllName>.dll
		- <PluginDllName>.pdb
		- <Dependency>.dll
```

�Զ�������Ŀ�ṹ
- WS.EmailSender
	- Plugin.cs
	- PluginConfig.cs
	- Basic
	- Controllers
	- Data
	- Managers
	- Stores
