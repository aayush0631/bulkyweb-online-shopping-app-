using FileSync.DataAccess.Data;
using FileSync.DataAccess.Repository.IRepository;
using FileSync.Models.Models;

namespace FileSync.DataAccess.Repository;

public class CopyHistoryRepository : Repository<CopyHistory>, ICopyHistoryRepository
{
	private readonly ApplicationDbContext _db;

	public CopyHistoryRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}

	public void Update(CopyHistory task)
	{
		_db.CopyHistories.Update(task);
	}
}