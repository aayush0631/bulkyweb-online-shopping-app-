using FileSync.Models.Models;
using FileSync.Services.Interface;

namespace FileSync.Services.Implementations;

public class SchedulerService : ISchedulerService
{
    private readonly ISyncTaskService _taskService;
    private readonly IConnectionServiceFactory _connectionServiceFactory;
    private readonly IFileCopyServiceFactory _fileCopyServiceFactory;
    private readonly ICredentialService _credentialService;

    public SchedulerService(
        ISyncTaskService taskService,
        IConnectionServiceFactory connectionServiceFactory,
        IFileCopyServiceFactory fileCopyServiceFactory,
        ICredentialService credentialService)
    {
        _taskService = taskService;
        _connectionServiceFactory = connectionServiceFactory;
        _fileCopyServiceFactory = fileCopyServiceFactory;
        _credentialService = credentialService;
    }

    public async Task ExecutePendingTasksAsync(CancellationToken cancellationToken)
    {
        var tasks = await _taskService.GetAllAsync();

        foreach (var task in tasks)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            if (!task.IsEnabled)
                continue;

            if (task.Schedule == null)
                continue;

            if (task.Schedule.StartTime > DateTime.Now)
                continue;

            INetworkConnectionService? connectionService = null;
            try
            {
                var credential = await _credentialService.GetByIdAsync(task.CredentialId);
                if (credential == null)
                    continue;

                // Dynamically resolve connection and file copy services
                connectionService = _connectionServiceFactory.Create(credential.Protocol);
                var fileCopyService = _fileCopyServiceFactory.Create(credential.Protocol);

                bool connected = await connectionService.ConnectAsync(credential);
                if (!connected)
                    continue;

                // Ensure task's credential navigation property is populated for FtpFileCopyService
                task.Credential = credential;

                await fileCopyService.CopyAsync(task, cancellationToken);

                await connectionService.DisconnectAsync();

                // TODO: Save CopyHistory, Update LastRunTime, Calculate NextRunTime
            }
            catch
            {
                if (connectionService != null)
                {
                    await connectionService.DisconnectAsync();
                }
            }
        }
    }
}