namespace SftpFileSync.Worker.Services;

public interface ISftpSyncService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}