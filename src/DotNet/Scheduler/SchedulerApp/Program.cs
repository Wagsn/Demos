using Hangfire;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace SchedulerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // - Thread
            new System.Threading.Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine(DateTime.Now + " -- Action System.Threading.Thread");
                    System.Threading.Thread.Sleep(1 * 1000);
                    Console.WriteLine(DateTime.Now + " -- Action System.Threading.Thread  -- Take 1 second");
                    System.Threading.Thread.Sleep(1 * 1000);
                }
            }).Start();

            // - Task 单独执行直接结束了应用
            Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine(DateTime.Now + " -- Action System.Threading.Tasks.Task");
                    System.Threading.Thread.Sleep(1 * 1000);
                    Console.WriteLine(DateTime.Now + " -- Action System.Threading.Tasks.Task  -- Take 1 second");
                    System.Threading.Thread.Sleep(1 * 1000);
                }
            });

            // - Timer
            var timer = new System.Timers.Timer(1 * 1000) { AutoReset = true };
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                Console.WriteLine(DateTime.Now + " -- Action System.Timers.Timer");
                System.Threading.Thread.Sleep(1 * 1000);
                Console.WriteLine(DateTime.Now + " -- Action System.Timers.Timer  -- Take 1 second");
            };
            timer.Start();

            // - Quartz
            IJobDetail job = JobBuilder.Create(typeof(TestJob))
                .WithIdentity(nameof(TestJob), nameof(TestJob))
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(nameof(TestJob), nameof(TestJob))
                .ForJob(job)
                .StartNow()
                .WithCronSchedule("*/1 * * * * ?")
                .Build();
            var scheduler = new StdSchedulerFactory().GetScheduler().Result;
            scheduler.ScheduleJob(job, trigger).Wait();
            scheduler.Start();

            // - Hangfire 最小间隔15s
            new System.Threading.Thread(() =>
            {
                #region 数据库连接和启动服务器
                var msSqlConnStr = "Server=.;Integrated Security=true;Database=HangfireTest;";
                GlobalConfiguration.Configuration.UseSqlServerStorage(msSqlConnStr);
                using var server = new BackgroundJobServer();
                #endregion

                RecurringJob.AddOrUpdate("test", () => HangfireRun(), () => "*/15 * * * * ?");
                
                while (true) Console.Read();
            }).Start();

            // - XxlJob

        }

        public static void HangfireRun()
        {
            Console.WriteLine(DateTime.Now + " -- Action Hangfire");
            System.Threading.Thread.Sleep(1 * 1000);
            Console.WriteLine(DateTime.Now + " -- Action Hangfire  -- Take 15 second");
        }
    }

    public class TestJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine(DateTime.Now + " -- Action Quartz");
                System.Threading.Thread.Sleep(1 * 1000);
                Console.WriteLine(DateTime.Now + " -- Action Quartz  -- Take 1 second");
            });
        }
    }
}
