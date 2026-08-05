using FileSync.Models.Models;

namespace FileSync.DataAccess.Repository.IRepository;

public interface ICredentialRepository : IRepository<Credential>
{
	void Update(Credential task);
}