using FileSync.DataAccess.Data;
using FileSync.DataAccess.Repository.IRepository;
using FileSync.Models.Models;

namespace FileSync.DataAccess.Repository;

public class CredentialRepository : Repository<Credential>, ICredentialRepository
{
	private readonly ApplicationDbContext _db;

	public CredentialRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}

	public void Update(Credential task)
	{
		_db.Credentials.Update(task);
	}
}