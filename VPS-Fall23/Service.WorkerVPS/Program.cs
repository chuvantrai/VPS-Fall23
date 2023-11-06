using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS;
using Service.WorkerVPS.BackgroundServices.ParkingZoneBackgroundServices;
using Service.WorkerVPS.Brokers;
using Service.WorkerVPS.Brokers.ParkingZoneJobBrokers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<AutoDeleteParkingZoneBackgroundService>();
        services.AddSingleton<IJobFactory, JobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddSingleton<QuartzServices>();
        services.AddSingleton<IRabbitMQClient, AutoDeleteParkingZoneDequeue>();
    })
    .Build();

await host.RunAsync();
