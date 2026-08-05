using FileSync.DataAccess.Data;
using FileSync.DataAccess.Repository.IRepository;
using FileSync.Models.Models;

namespace FileSync.DataAccess.Repository;

public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
{
	private readonly ApplicationDbContext _db;

	public ScheduleRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}

	public void Update(Schedule task)
	{
		_db.Schedules.Update(task);
	}
}