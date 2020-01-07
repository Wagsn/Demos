using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalFolderWatcherDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var pluginDir = Path.Combine(AppContext.BaseDirectory, "Plugins");
            if (!Directory.Exists(pluginDir)) Directory.CreateDirectory(pluginDir);
            IFileProvider fileProvider = new PhysicalFileProvider(pluginDir);
            // 监听文件夹 子文件夹内发生的任意修改将不会触发 子文件内容发生变化将不会触发
            //OldFiles.AddRange(fileProvider.GetDirectoryContents("").Where(f => f.IsDirectory));
            OldFiles.AddRange(fileProvider.GetDirectoryContents("*").Where(f => f.IsDirectory));
            ChangeToken.OnChange(() => fileProvider.Watch("*"), () =>
            {
                var fileInfos = fileProvider.GetDirectoryContents("*").Where(f => f.IsDirectory);
                // 删除的文件
                var delFiles = OldFiles.Where(a => !fileInfos.Any(b => b.PhysicalPath == a.PhysicalPath));
                // 添加的文件
                var addFiles = fileInfos.Where(a => !OldFiles.Any(b => b.PhysicalPath == a.PhysicalPath));
                // 修改的文件 判断：新文件中有并且修改时间大于旧文件
                var modFiles = OldFiles.Where(a => fileInfos.FirstOrDefault(b => b.PhysicalPath == a.PhysicalPath)?.LastModified > a.LastModified);

                var allFiles = delFiles.ToDictionary(a => a.Name, status => 3)
                .Union(addFiles.ToDictionary(a => a.Name, status => 1))
                .Union(modFiles.ToDictionary(a => a.Name, status => 2));
                var statusDic = new Dictionary<int, string> { { 1, "Add" }, { 2, "Mod" }, { 3, "Del" } };
                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.FFFFFF")} {string.Join("\r\n", allFiles.Select(kv => $"{kv.Key} : {statusDic[kv.Value]}"))}");

                // 最后文件列表刷新
                OldFiles.Clear();
                OldFiles.AddRange(fileInfos);
            });
            while (true)
            {
                Task.Delay(5 * 1000).Wait();
            }
        }

        public static List<IFileInfo> OldFiles { get; } = new List<IFileInfo>();

        public static void PrintDllNames(IEnumerable<string> names)
        {
            Console.WriteLine($"- {DateTime.Now.ToString("HH:mm:ss.FFFFFF")} Plugin Dlls {names.Count()}\r\n{string.Join("\r\n", names)}\r\n");
        }

        //ChangeToken.OnChange(() => fileProvider.Watch("Data.txt"), () => LoadFileAsync(fileProvider));
        public static async void LoadFileAsync(IFileProvider fileProvider)
        {
            Stream stream = fileProvider.GetFileInfo("Data.txt").CreateReadStream();
            {
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                Console.WriteLine(Encoding.UTF8.GetString(buffer));
            }
        }
    }
}
