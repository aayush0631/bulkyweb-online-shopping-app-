using FileSync.Models.Models;

namespace FileSync.DataAccess.Repository.IRepository;

public interface ISyncTaskRepository : IRepository<SyncTask>
{
	void Update(SyncTask task);
}