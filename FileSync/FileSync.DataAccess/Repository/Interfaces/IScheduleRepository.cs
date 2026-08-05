using FileSync.Models.Models;

namespace FileSync.DataAccess.Repository.IRepository;

public interface IScheduleRepository : IRepository<Schedule>
{
	void Update(Schedule task);
}