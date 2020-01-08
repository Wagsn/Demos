using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalFolderWatcherDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var pluginDir = Path.Combine(AppContext.BaseDirectory, "Plugins");
            if (!Directory.Exists(pluginDir)) Directory.CreateDirectory(pluginDir);
            // 监听子文件夹内容的变化
            WatchSubfolderChange(pluginDir, (modPluginDirs) =>
            {
                Console.WriteLine($"- {DateTime.Now.ToString("HH:mm:ss.FFFFFF")}\r\n{string.Join("\r\n", modPluginDirs.Select(a => $"{a} : Mod"))}");
            });
            while (true)
            {
                Task.Delay(5 * 1000).Wait();
            }
        }

        /// <summary>
        /// 监听子文件夹变化（当子文件夹中的文件发生改变）
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="onChange4Subfolder"></param>
        public static void WatchSubfolderChange(string rootPath, Action<IEnumerable<string>> onChange4Subfolder)
        {
            PhysicalFileProvider fileProvider = new PhysicalFileProvider(rootPath);
            // 递归查询所有子文件
            OldFiles.AddRange(GetAllSubFiles(fileProvider, "").Where(f => !f.IsDirectory));
            // 监听监听所有文件，当文件发生修改时反应为某个子文件夹的修改，在最后将修改的文件夹路径打印出来
            // Example: **/*.cs, *.*, subFolder/**/*.cshtml.
            ChangeToken.OnChange(() => fileProvider.Watch("*"), () =>
            {
                // 递归查询所有子文件
                var files = GetAllSubFiles(fileProvider, "").Where(f => !f.IsDirectory);
                // 查询根目录下的子文件夹
                var subfolders = fileProvider.GetDirectoryContents("");
                // 变化的文件
                var modFiles = OldFiles.Where(a => (!files.Any(b => b.PhysicalPath == a.PhysicalPath))
                        || files.FirstOrDefault(b => b.PhysicalPath == a.PhysicalPath)?.LastModified > a.LastModified)
                    .Union(files.Where(a => !OldFiles.Any(b => b.PhysicalPath == a.PhysicalPath)))
                    .Select(f => f.PhysicalPath);

                // 发生变化的文件夹
                var modPluginDirs = subfolders.Select(f => f.PhysicalPath).Where(a => modFiles.Any(b => b.Contains(a)));

                // 修改回调
                if(modPluginDirs.Any()) onChange4Subfolder(modPluginDirs);

                // 最后文件列表刷新
                OldFiles.Clear();
                OldFiles.AddRange(files);
            });
        }

        public static List<IFileInfo> OldFiles { get; } = new List<IFileInfo>();

        private static IEnumerable<IFileInfo> GetAllSubFiles(PhysicalFileProvider fileProvider, string subPath)
        {
            var rootPath = fileProvider.Root;
            List<IFileInfo> fileInfos = new List<IFileInfo>();
            var files = fileProvider.GetDirectoryContents(subPath).ToList();
            foreach (var dir in files.Where(a => a.IsDirectory))
            {
                fileInfos.AddRange(GetAllSubFiles(fileProvider, dir.PhysicalPath.Substring(rootPath.Length)));
            }
            fileInfos.AddRange(files);
            return fileInfos;
        }
    }
}
