namespace FileSync.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ISyncTaskRepository SyncTask { get; }

        ICredentialRepository Credential { get; }

        IScheduleRepository Schedule { get; }

        ICopyHistoryRepository CopyHistory { get; }
        void Save();
    }
}
