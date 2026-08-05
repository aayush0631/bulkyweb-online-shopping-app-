using FileSync.DataAccess.Repository;
using FileSync.DataAccess.Repository.IRepository;
using FileSync.Models.Models;
using System.Runtime.CompilerServices;

namespace FileSync.Services.Implementations;

public class SyncTaskService : ISyncTaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public SyncTaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<IEnumerable<SyncTask>> GetAllAsync()
    {
        var tasks = _unitOfWork.SyncTask.GetAll(includeProperties: "Schedule,Credential");
        return Task.FromResult(tasks);
    }

    public Task<SyncTask?> GetByIdAsync(int id)
    {
        var task = _unitOfWork.SyncTask.Get(t => t.Id == id, includeProperties: "Schedule,Credential");
        return Task.FromResult(task);
    }

    public Task CreateAsync(SyncTask task)
    {
        if (string.IsNullOrWhiteSpace(task.TaskName))
            throw new ArgumentException("Task name is required.");

        if (string.IsNullOrWhiteSpace(task.RemoteRelativePath))
            throw new ArgumentException("Remote path is required.");

        if (string.IsNullOrWhiteSpace(task.LocalPath))
            throw new ArgumentException("Local path is required.");

        _unitOfWork.SyncTask.Add(task);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }

    public Task UpdateAsync(SyncTask task)
    {
        _unitOfWork.SyncTask.Update(task);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var task = _unitOfWork.SyncTask.Get(t => t.Id == id);

        if (task == null)
            throw new Exception("Task not found.");

        _unitOfWork.SyncTask.Remove(task);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }
}