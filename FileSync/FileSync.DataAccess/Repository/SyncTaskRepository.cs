using FileSync.DataAccess.Data;
using FileSync.DataAccess.Repository.IRepository;
using FileSync.Models.Models;

namespace FileSync.DataAccess.Repository;

public class SyncTaskRepository : Repository<SyncTask>, ISyncTaskRepository
{
	private readonly ApplicationDbContext _db;

	public SyncTaskRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}

	public void Update(SyncTask task)
	{
		_db.SyncTasks.Update(task);
	}
}