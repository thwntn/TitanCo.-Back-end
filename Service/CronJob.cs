namespace ReferenceFeature;

public class CronJobService() : IJob
{
    private DatabaseContext _databaseContext;
    private IPlanning _planningService;

    public Task Execute(IJobExecutionContext jobExecutionContext)
    {
        var serviceProvider = (ServiceProvider)jobExecutionContext.MergedJobDataMap[nameof(ServiceProvider)];
        _databaseContext = serviceProvider.GetService<DatabaseContext>();
        _planningService = serviceProvider.GetService<IPlanning>();

        try
        {
            Logging();
        }
        catch (Exception e)
        {
            Logger.Log(e);
        }
        return Task.CompletedTask;
    }

    public void Logging()
    {
        _planningService.SendNotiOrMail();
    }
}
