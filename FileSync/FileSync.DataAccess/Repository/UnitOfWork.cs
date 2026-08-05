using FileSync.DataAccess.Data;
using FileSync.DataAccess.Repository.IRepository;

namespace FileSync.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ISyncTaskRepository SyncTask { get; }

        public ICredentialRepository Credential { get; }

        public IScheduleRepository Schedule { get; }

        public ICopyHistoryRepository CopyHistory { get; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db= db;
            SyncTask = new SyncTaskRepository(db);
            Credential = new CredentialRepository(db);
            Schedule = new ScheduleRepository(db);
            CopyHistory = new CopyHistoryRepository(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
