using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.BackgroundServices.ParkingZoneBackgroundServices;
using Service.WorkerVPS.BackgroundServices.Smtp;
using Service.WorkerVPS.Brokers.ParkingZoneJobBrokers;
using Service.WorkerVPS.Brokers.SmtpBrokers;
using Service.WorkerVPS.HostingBackgroundServices.ParkingZone;
using Service.WorkerVPS.Models;
using Service.WorkerVPS.Services.RepeatingService;
using Service.WorkerVPS.Services.Smtp;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<RabbitMQProfile>(context.Configuration.GetSection("RabbitMQ"));
        services.Configure<HostEmailProfile>(context.Configuration.GetSection("HostEmail"));
        services.AddHostedService<CreateDeletingPZJobBackgroundService>();
        services.AddHostedService<CreateCancelBookingJobBackgroundService>();
        services.AddHostedService<RemoveCancelBookingJobBackgroundService>();
        services.AddHostedService<RemoveDeletingPZJobBackGroundService>();
        services.AddHostedService<SmtpBackgroundService>();
        services.AddHostedService<SendPromoCode>();
        services.AddSingleton<IJobFactory, JobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddSingleton<ISmtpService, SmtpServices>();
        services.AddSingleton<QuartzServices>();
        services.AddSingleton<CreateDeletingPZJobDequeue>();
        services.AddSingleton<RemoveDeletingPZJobDequeue>();
        services.AddSingleton<CreateCancelBookingDequeue>();
        services.AddSingleton<RemoveCancelBookingDequeue>();
        services.AddSingleton<SendMailDequeue>();
    })
    .Build();

await host.RunAsync();
