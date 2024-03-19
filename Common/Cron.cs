namespace ReferenceFeature;

public class CronJob
{
    public static async void Configure(WebApplicationBuilder builder)
    {
        ServiceProvider provider = builder.Services.BuildServiceProvider();
        DatabaseContext databaseContext = provider.GetService<DatabaseContext>();
        IRole roleService = provider.GetService<IRole>();

        IJobDetail job = JobBuilder.Create<CronJobService>().WithIdentity(nameof(CronJobService)).Build();
        job.JobDataMap.Put(nameof(ServiceProvider), provider);

        ITrigger trigger = TriggerBuilder
            .Create()
            .WithIdentity(nameof(CronJobService))
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())
            .Build();

        Quartz.Impl.StdSchedulerFactory schedulerFactory = SchedulerBuilder.Create().Build();
        IScheduler scheduler = await schedulerFactory.GetScheduler();
        await scheduler.ScheduleJob(job, trigger);

        // auto.migration
        await databaseContext.Database.MigrateAsync();

        roleService.Sync();
        await scheduler.Start();
    }
}
