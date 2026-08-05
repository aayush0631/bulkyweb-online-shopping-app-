using FileSync.Models.Models;
using Microsoft.Extensions.Logging;

namespace FileSync.Services.Implementations;

public class FileCopyService : IFileCopyService
{
    private readonly ILogger<FileCopyService> _logger;

    public FileCopyService(ILogger<FileCopyService> logger)
    {
        _logger = logger;
    }

    public async Task CopyAsync(
        SyncTask task,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Ensure destination directory exists
            string? directory = Path.GetDirectoryName(task.LocalPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // If file exists and should be skipped
            if (task.SkipIfExists && File.Exists(task.LocalPath))
            {
                if (await VerifyCopyAsync(task))
                {
                    _logger.LogInformation(
                        "Skipping copy. File already exists: {File}",
                        task.LocalPath);

                    return;
                }
            }

            using FileStream source = new(
                task.RemoteRelativePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            using FileStream destination = new(
                task.LocalPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None);

            await source.CopyToAsync(destination, cancellationToken);

            _logger.LogInformation(
                "Copied file from {Source} to {Destination}",
                task.RemoteRelativePath,
                task.LocalPath);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Copy operation cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error copying file from {Source} to {Destination}",
                task.RemoteRelativePath,
                task.LocalPath);

            throw;
        }
    }

    public async Task ResumeCopyAsync(
        SyncTask task,
        CancellationToken cancellationToken = default)
    {
        try
        {
            string? directory = Path.GetDirectoryName(task.LocalPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using FileStream source = new(
                task.RemoteRelativePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            using FileStream destination = new(
                task.LocalPath,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.None);

            // Destination is corrupted (larger than source)
            if (destination.Length > source.Length)
            {
                _logger.LogWarning(
                    "Destination file is larger than source. Restarting copy.");

                destination.SetLength(0);
            }

            source.Position = destination.Length;
            destination.Position = destination.Length;

            await source.CopyToAsync(destination, cancellationToken);

            _logger.LogInformation(
                "Resume completed for {File}",
                task.LocalPath);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Resume operation cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Resume copy failed.");

            throw;
        }
    }

    public Task<bool> DestinationExistsAsync(SyncTask task)
    {
        return Task.FromResult(File.Exists(task.LocalPath));
    }

    public Task<bool> VerifyCopyAsync(SyncTask task)
    {
        var source = new FileInfo(task.RemoteRelativePath);
        var destination = new FileInfo(task.LocalPath);

        bool result =
            source.Exists &&
            destination.Exists &&
            source.Length == destination.Length;

        return Task.FromResult(result);
    }
}