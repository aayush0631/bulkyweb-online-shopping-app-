using FileSync.Models.Models;

public interface IFileCopyService
{
    Task CopyAsync(SyncTask task,CancellationToken cancellationToken = default);

    Task ResumeCopyAsync(SyncTask task, CancellationToken cancellationToken = default);

    Task<bool> DestinationExistsAsync(SyncTask task);

    Task<bool> VerifyCopyAsync(SyncTask task);
}