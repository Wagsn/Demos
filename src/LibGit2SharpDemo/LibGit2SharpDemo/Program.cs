using System;
using System.Linq;

namespace LibGit2SharpDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Clone 远程仓库
            var remoteWorkdirPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Repositories", "github.com", "Wagsn", "Demos");
            if ((!System.IO.Directory.Exists(remoteWorkdirPath)) || !System.IO.Directory.EnumerateFileSystemEntries(remoteWorkdirPath).Any())
                LibGit2Sharp.Repository.Clone("https://github.com/Wagsn/Demos.git", remoteWorkdirPath);

            var localWorkdirPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Repositories", "localhost", "Wagsn", "Demo");
            if ((!System.IO.Directory.Exists(localWorkdirPath)) || !System.IO.Directory.EnumerateFileSystemEntries(localWorkdirPath).Any())
            {
                // 创建本地仓库
                LibGit2Sharp.Repository.Init(localWorkdirPath);

                using (var repo = new LibGit2Sharp.Repository(localWorkdirPath))
                {
                    // 打印分支
                    var branches = repo.Branches;
                    foreach (var b in branches)
                    {
                        Console.WriteLine(b.FriendlyName);
                    }

                    // 暂存更改
                    System.IO.File.WriteAllText(System.IO.Path.Combine(localWorkdirPath, "README.md"), "# README\r\n");
                    Console.WriteLine("- 暂存更改");
                    LibGit2Sharp.Commands.Stage(repo, "*");
                    // 提交更改
                    Console.WriteLine("- 提交更改");
                    var commit = repo.Commit("Add README.md", new LibGit2Sharp.Signature("Wagsn", "wagsn@foxmail.com", DateTimeOffset.Now), new LibGit2Sharp.Signature("Wagsn", "wagsn@foxmail.com", DateTimeOffset.Now), new LibGit2Sharp.CommitOptions());

                    // 打印记录
                    Console.WriteLine("- 打印记录");
                    Console.WriteLine("---\r\n" + repo.Head.Tip.Message + "\r\n---");

                    Console.WriteLine("- 提交更改");
                    System.IO.File.WriteAllText(System.IO.Path.Combine(localWorkdirPath, "README.md"), "# README\r\nUpdate");
                    LibGit2Sharp.Commands.Stage(repo, "*");
                    repo.Commit("Update README.md", new LibGit2Sharp.Signature("Wagsn", "wagsn@foxmail.com", DateTimeOffset.Now), new LibGit2Sharp.Signature("Wagsn", "wagsn@foxmail.com", DateTimeOffset.Now), new LibGit2Sharp.CommitOptions());

                    // 打印记录
                    Console.WriteLine("- 打印记录");
                    Console.WriteLine("---\r\n"+repo.Head.Tip.Message+"\r\n---");
                    // 撤销更改
                    Console.WriteLine("- 撤销更改");
                    Console.WriteLine(repo.Head.Commits.FirstOrDefault()?.Message);
                    if(repo.Head.Commits.Any()) repo.Reset(LibGit2Sharp.ResetMode.Hard, commit);
                    // 打印记录
                    Console.WriteLine("- 打印记录");
                    Console.WriteLine("---\r\n" + repo.Head.Tip.Message + "\r\n---");
                }
            }
        }
    }
}
