# README 

## 第三方DLL引入方式

- 独立项目引用（推荐）

将需要引入第三方DLL的代码放到一个独立的项目的根目录下，并设置DLL较新则复制，然后主项目引用该项目，注意平台CPU

- 修改 exe.config

```exe.config
<runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
        <publisherPolicy apply="yes" />
        <probing privatePath="dll_lib" /> <!--相对debug目录的路径-->
    </assemblyBinding>
</runtime>
```

- 启动时将相对路径引入到PATH环境变量中

```cs
    /// <summary>
    /// 添加环境变量
    /// </summary>
    /// <param name="paths">路径列表</param>
    internal static void AddEnvironmentPaths(IEnumerable<string> paths)
    {          
        var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };
        string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(paths));
        Environment.SetEnvironmentVariable("PATH", newPath);   // 这种方式只会修改当前进程的环境变量
    }
```

- 在导入的地方使用绝对路径（不推荐）

```cs
[DllImport("C:\Windows\System32\kernel32.dll")]
private extern static bool FreeLibrary(IntPtr lib);
```

- 将DLL丢入到系统PATH中（不推荐）

`C:\Windows\System32\`


- 手动加载反射调用dll

- 修改csproj文件，隐式依赖dll

