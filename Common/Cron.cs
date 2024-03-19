namespace ReferenceFeature;

public class CronJob
{
    public static async void Configure(WebApplicationBuilder builder)
    {
        ServiceProvider provider = builder.Services.BuildServiceProvider();

        IJobDetail job = JobBuilder.Create<CronJobService>().WithIdentity(nameof(CronJobService)).Build();
        job.JobDataMap.Put(nameof(ServiceProvider), provider);

        ITrigger trigger = TriggerBuilder
            .Create()
            .WithIdentity(nameof(CronJobService))
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())
            .Build();

        var schedulerFactory = SchedulerBuilder.Create().Build();
        var scheduler = await schedulerFactory.GetScheduler();
        await scheduler.ScheduleJob(job, trigger);

        IRole roleService = provider.GetService<IRole>();
        if (Convert.ToBoolean(Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Migration))) is false)
        {
            roleService.Sync();
            await scheduler.Start();
        }
    }
}
