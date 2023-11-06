using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IJobFactory, JobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddSingleton<QuartzServices>();
    })
    .Build();

await host.RunAsync();
