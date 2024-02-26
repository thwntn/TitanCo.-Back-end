using System.Configuration;
using Microsoft.Extensions.Options;

namespace ReferenceFeature;

public class CronJob
{
    public static async void Configure(WebApplicationBuilder builder)
    {
        var job = JobBuilder.Create<CronJobService>().WithIdentity(nameof(CronJobService)).Build();
        job.JobDataMap.Put(nameof(ServiceProvider), builder.Services.BuildServiceProvider());

        var trigger = TriggerBuilder
            .Create()
            .WithIdentity(nameof(CronJobService))
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())
            .Build();

        var schedulerFactory = SchedulerBuilder.Create().Build();
        var scheduler = await schedulerFactory.GetScheduler();
        await scheduler.ScheduleJob(job, trigger);

        await scheduler.Start();
    }
}
