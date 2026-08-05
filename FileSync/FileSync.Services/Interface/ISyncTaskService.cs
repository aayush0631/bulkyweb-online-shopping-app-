using FileSync.Models.Models;

public interface ISyncTaskService
{
    Task<IEnumerable<SyncTask>> GetAllAsync();

    Task<SyncTask?> GetByIdAsync(int id);

    Task CreateAsync(SyncTask task);

    Task UpdateAsync(SyncTask task);

    Task DeleteAsync(int id);
}