using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PhysicalFolderWatcherDemo
{
    public class SubfolderChangeWatcher
    {
        /// <summary>
        /// 监听子文件夹变化
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="onChange">修改回调（子文件夹中的文件发生变化）</param>
        public static void WatchSubfolderChange(string rootPath, Action<IEnumerable<string>> onChange)
        {
            PhysicalFileProvider fileProvider = new PhysicalFileProvider(rootPath);
            // 递归查询所有子文件
            OldFiles.AddRange(fileProvider.GetDirectoryContents("").Where(f => !f.IsDirectory));
            // 监听监听所有文件，当文件发生修改时反应为某个子文件夹的修改，在最后将修改的文件夹路径打印出来
            ChangeToken.OnChange(() => fileProvider.Watch("*"), () =>  // filter Example: **/*.cs, *.*, subFolder/**/*.cshtml.
            {
                // 查询所有子文件
                var files = fileProvider.GetDirectoryDepthContents("").Where(f => !f.IsDirectory);
                // 查询变化的文件
                var modFiles = OldFiles.Where(a => (!files.Any(b => b.PhysicalPath == a.PhysicalPath))
                        || files.FirstOrDefault(b => b.PhysicalPath == a.PhysicalPath)?.LastModified > a.LastModified)
                    .Union(files.Where(a => !OldFiles.Any(b => b.PhysicalPath == a.PhysicalPath)))
                    .Select(f => f.PhysicalPath);
                // 查询根目录下的子文件夹
                var subfolders = fileProvider.GetDirectoryContents("");
                // 查询发生变化的文件夹
                var modDirs = subfolders.Select(f => f.PhysicalPath).Where(a => modFiles.Any(b => b.Contains(a)));
                // 有文件夹修改则回调
                if (modDirs.Any()) onChange(modDirs);
                // 刷新暂存文件列表
                OldFiles.Refresh(files);
            });
        }
        /// <summary>
        /// 暂存旧文件列表以作比较
        /// </summary>
        private static List<IFileInfo> OldFiles { get; } = new List<IFileInfo>();
    }
}
