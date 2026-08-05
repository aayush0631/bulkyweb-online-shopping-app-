using FileSync.Models.Models;

namespace FileSync.DataAccess.Repository.IRepository;

public interface ICopyHistoryRepository : IRepository<CopyHistory>
{
	void Update(CopyHistory task);
}