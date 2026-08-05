using System.Threading;
using System.Threading.Tasks;

namespace FileSync.Services.Interface;

public interface ISchedulerService
{
    Task ExecutePendingTasksAsync(CancellationToken cancellationToken);
}