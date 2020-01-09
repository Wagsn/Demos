using System;
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
            using var watcher = SubfolderChangeWatcher.Watch(pluginDir, (modPluginDirs) =>
            {
                Console.WriteLine($"- {DateTime.Now.ToString("HH:mm:ss.FFFFFF")}\r\n{string.Join("\r\n", modPluginDirs.Select(a => $"{a} : Mod"))}");
            });
            while (true)
            {
                Task.Delay(5 * 1000).Wait();
            }
        }
    }
}
