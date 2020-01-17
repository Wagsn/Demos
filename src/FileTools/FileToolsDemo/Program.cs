using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace FileToolsDemo
{
    static class Program
    {
        static void Main(string[] args)
        {
            //var root = AppContext.BaseDirectory;
            //CreateTestData(root);
            var root = @"";
            LowerFileLevel(root);
            //MoveFilesToTop(root);
            //Rename(root, @"(\\19\.)", @"\2019.");

            //var root = AppContext.BaseDirectory;
            //var first = Directory.EnumerateFiles(root).FirstOrDefault();
            //if (first != null)
            //{
            //    Console.WriteLine($"path: {first}");
            //    Console.WriteLine($"hash: {GetFileHash(path:first)}");
            //    Console.WriteLine($"sign: {GetFileSignature(path:first)}");
            //}
        }

        /// <summary>
        /// 批量正则修改文件夹下的文件名
        /// </summary>
        /// <param name="root"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        public static void Rename(string root, string pattern, string replacement)
        {
            var files = Directory.EnumerateFiles(root);
            var outs = new Dictionary<string, string>();
            foreach(var path in files)
            {
                outs[path] = Regex.Replace(path, pattern, replacement);
            }
            foreach(var kv in outs)
            {
                if (File.Exists(kv.Value))
                {
                    continue;
                }
                Console.WriteLine($"Rename from {kv.Key} to {kv.Value}");
                File.Move(kv.Key, kv.Value);
                Console.WriteLine("OK");
            }
        }

        /// <summary>
        /// 创建测试数据
        /// </summary>
        /// <param name="root"></param>
        static void CreateTestData(string root)
        {
            if(root == null)
            {
                root = AppContext.BaseDirectory;
            }
            var subFolder = Path.Combine(root, "2018");
            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }
            var filePath = Path.Combine(subFolder, "01.ext");
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "2018_01.ext");
            }
        }

        /// <summary>
        /// 获取文件特征
        /// <HASH>_<媒体类型>_<媒体特征>
        /// 如果HASH一致表示完全相同的两个文件
        /// 然后判断媒体类型，媒体类型一致则判断媒体特征，计算媒体特征的相似度
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string GetFileSignature(string path)
        {
            return path;
        }

        /// <summary>
        /// 获取文件的HASH码
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        static string GetFileHash(string path)
        {
            // 获取HASH MD5.Create()
            var hash = SHA256.Create();
            using var stream = new FileStream(path, FileMode.Open);
            byte[] hashByte = hash.ComputeHash(stream);
            stream.Close();
            return BitConverter.ToString(hashByte).Replace("-", "");
        }

        /// <summary>
        /// 移动一级文件夹中的文件到根文件夹中
        /// </summary>
        /// <param name="root"></param>
        static void MoveFilesToTop(string root)
        {
            // 根路径下的文件夹
            var rootDirs = Directory.EnumerateDirectories(root);
            // 根路径文件夹对应的子文件
            // root = "/" subs.Keys = "/2018" subs[key] = ["/2018/01.ext","/2018/02.ext"]
            var subs = new Dictionary<string, List<string>>();
            foreach (var item in rootDirs)
            {
                subs[item] = Directory.EnumerateFiles(item).ToList();
            }
            // outputs key = "/2018/01.ext" value = "/01.ext"
            var outputs = new Dictionary<string, string>();
            foreach (var folder in subs.Keys)
            {
                foreach (var path in subs[folder])
                {
                    var name = Path.GetFileName(path);
                    // root = "/" path = "/2018/01.ext" outPath = "/01.ext"
                    var outPath = Path.Combine(root, name);
                    outputs[path] = outPath;
                }
            }
            // 改名操作
            foreach (var kv in outputs)
            {
                if (File.Exists(kv.Value))
                {
                    continue;
                }
                File.Move(kv.Key, kv.Value);
            }
        }

        /// <summary>
        /// 降低文件的层级
        /// 如果文件名不带其上级文件夹名称前缀则追加，否则不追加
        /// </summary>
        /// <param name="root">根路径；工作路径</param>
        static void LowerFileLevel(string root)
        {
            // 根路径下的文件夹
            var rootDirs = Directory.EnumerateDirectories(root);
            // 根路径文件夹对应的子文件[夹]
            var subs = new Dictionary<string, List<string>>();
            foreach(var item in rootDirs)
            {
                subs[item] = Directory.EnumerateFiles(item).ToList();
            }
            // 构建改名路径 "/2018/01.ext" -> "/2018_01.ext"  "2018/01/02.ext"->"/2018_01/02.ext"
            // outputs key = "/2018/01.ext" value = "/2018_01.ext"
            var outputs = new Dictionary<string, string>();
            foreach(var folder in subs.Keys)
            {
                foreach(var path in subs[folder])
                {
                    // root = "/" subs.Keys = "/2018" subs[key] = "/2018/01.ext"
                    // dirSec = "2018"
                    var prefix = root.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                    var dirSec = folder.Replace(prefix, "");
                    var name = Path.GetFileName(path);
                    var outName = name.StartsWith(dirSec) ? name : (dirSec + "_" + name);
                    var outPath = Path.Combine(root, outName);
                    outputs[path] = outPath;
                }
            }
            // 改名操作
            foreach(var kv in outputs)
            {
                if (File.Exists(kv.Value))
                {
                    continue;
                }
                Console.WriteLine($"move {kv.Key} to {kv.Value}");
                File.Move(kv.Key, kv.Value);
                Console.WriteLine("ok");
            }
        }
    }
}
