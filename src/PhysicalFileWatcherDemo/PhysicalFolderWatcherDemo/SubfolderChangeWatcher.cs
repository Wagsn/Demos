using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhysicalFolderWatcherDemo
{
    /// <summary>
    /// 子文件夹内容变化监听器
    /// </summary>
    public class SubfolderChangeWatcher : IDisposable
    {
        /// <summary>
        /// Files
        /// </summary>
        private List<IFileInfo> Files { get; } = new List<IFileInfo>();
        /// <summary>
        /// File Provider
        /// </summary>
        private PhysicalFileProvider FileProvider { get; set; }
        /// <summary>
        /// Listen for changes in subfolders
        /// </summary>
        /// <param name="rootPath">监听根路径</param>
        /// <param name="onChange">修改回调（子文件夹中的文件发生变化）</param>
        public static SubfolderChangeWatcher Watch(string rootPath, Action<IEnumerable<string>> onChange)
        {
            SubfolderChangeWatcher watcher = new SubfolderChangeWatcher();
            watcher.SubfolderChangeWatch(rootPath, onChange);
            return watcher;
        }
        /// <summary>
        /// Listen for changes in subfolders
        /// </summary>
        /// <param name="rootPath">监听根路径</param>
        /// <param name="onChange">修改回调（子文件夹中的文件发生变化）</param>
        private void SubfolderChangeWatch(string rootPath, Action<IEnumerable<string>> onChange)
        {
            FileProvider = new PhysicalFileProvider(rootPath);
            // 递归查询所有子文件
            Files.AddRange(FileProvider.GetDirectoryContents("").Where(f => !f.IsDirectory));
            // 监听监听所有文件，当文件发生修改时反应为某个子文件夹的修改，在最后将修改的文件夹路径打印出来
            ChangeToken.OnChange(() => FileProvider.Watch("*"), () =>  // filter Example: **/*.cs, *.*, subFolder/**/*.cshtml.
            {
                // 查询所有子文件
                var files = FileProvider.GetDirectoryDepthContents("").Where(f => !f.IsDirectory);
                // 查询变化的文件
                var modFiles = Files.Where(a => (!files.Any(b => b.PhysicalPath == a.PhysicalPath))
                        || files.FirstOrDefault(b => b.PhysicalPath == a.PhysicalPath)?.LastModified > a.LastModified)
                    .Union(files.Where(a => !Files.Any(b => b.PhysicalPath == a.PhysicalPath)))
                    .Select(f => f.PhysicalPath);
                // 查询根目录下的子文件夹
                var subfolders = FileProvider.GetDirectoryContents("");
                // 查询发生变化的文件夹
                var modDirs = subfolders.Select(f => f.PhysicalPath).Where(a => modFiles.Any(b => b.Contains(a)));
                // 有文件夹修改则回调
                if (modDirs.Any()) onChange(modDirs);
                // 刷新暂存文件列表
                Files.Refresh(files);
            });
        }
        /// <summary>
        /// Disposes the watcher.
        /// </summary>
        public virtual void Dispose()
        {
            FileProvider?.Dispose();
            FileProvider = null;
            Files.Clear();
        }
        /// <summary>
        /// Disposes the watcher.
        /// </summary>
        /// <param name="disposing">true is invoked from System.IDisposable.Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) Dispose();
        }
    }
}
