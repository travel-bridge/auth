namespace Auth.Worker.Workers;

public abstract class WorkerBase : BackgroundService
{
    protected WorkerBase(ILogger<WorkerBase> logger)
    {
        Logger = logger;
    }

    protected ILogger<WorkerBase> Logger { get; }
    
    protected async Task ExecuteSafelyAsync(Func<Task> func, CancellationToken stoppingToken)
    {
        try
        {
            await func();
        }
        catch (TaskCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
        }
    }
}