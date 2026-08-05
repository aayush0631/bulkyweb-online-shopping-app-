using FileSync.Models.Models;
using Microsoft.Extensions.Logging;

namespace FileSync.Services.Implementations;

public class FtpFileCopyService : IFileCopyService
{
    private readonly ILogger<FtpFileCopyService> _logger;

    public FtpFileCopyService(ILogger<FtpFileCopyService> logger)
    {
        _logger = logger;
    }

    public async Task CopyAsync(
        SyncTask task,
        CancellationToken cancellationToken = default)
    {
        try
        {
            string ftpUri = BuildFtpUri(task);

            // Ensure local directory exists
            string? directory = Path.GetDirectoryName(task.LocalPath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            // Skip if file exists and is complete
            if (task.SkipIfExists && File.Exists(task.LocalPath))
            {
                if (await VerifyCopyAsync(task))
                {
                    _logger.LogInformation(
                        "Skipping FTP download. File already exists: {File}",
                        task.LocalPath);
                    return;
                }
            }

            var request = CreateRequest(task, ftpUri,
                System.Net.WebRequestMethods.Ftp.DownloadFile);

            using var response = (System.Net.FtpWebResponse)await request.GetResponseAsync();
            using var responseStream = response.GetResponseStream();
            using var fileStream = new FileStream(
                task.LocalPath, FileMode.Create, FileAccess.Write, FileShare.None);

            await responseStream.CopyToAsync(fileStream, cancellationToken);

            _logger.LogInformation(
                "FTP download complete: {Uri} → {Local}",
                ftpUri, task.LocalPath);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("FTP download cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "FTP download failed: {Remote} → {Local}",
                task.RemoteRelativePath, task.LocalPath);
            throw;
        }
    }

    public async Task ResumeCopyAsync(
        SyncTask task,
        CancellationToken cancellationToken = default)
    {
        try
        {
            string ftpUri = BuildFtpUri(task);

            string? directory = Path.GetDirectoryName(task.LocalPath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            long existingLength = 0;
            if (File.Exists(task.LocalPath))
                existingLength = new FileInfo(task.LocalPath).Length;

            var request = CreateRequest(task, ftpUri,
                System.Net.WebRequestMethods.Ftp.DownloadFile);
            request.ContentOffset = existingLength;

            using var response = (System.Net.FtpWebResponse)await request.GetResponseAsync();
            using var responseStream = response.GetResponseStream();
            using var fileStream = new FileStream(
                task.LocalPath, FileMode.Append, FileAccess.Write, FileShare.None);

            await responseStream.CopyToAsync(fileStream, cancellationToken);

            _logger.LogInformation(
                "FTP resume download complete (offset {Offset}): {Uri} → {Local}",
                existingLength, ftpUri, task.LocalPath);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("FTP resume download cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FTP resume download failed.");
            throw;
        }
    }

    public Task<bool> DestinationExistsAsync(SyncTask task)
    {
        return Task.FromResult(File.Exists(task.LocalPath));
    }

    public async Task<bool> VerifyCopyAsync(SyncTask task)
    {
        try
        {
            string ftpUri = BuildFtpUri(task);

            var request = CreateRequest(task, ftpUri,
                System.Net.WebRequestMethods.Ftp.GetFileSize);

            using var response = (System.Net.FtpWebResponse)await request.GetResponseAsync();
            long remoteSize = response.ContentLength;

            if (!File.Exists(task.LocalPath))
                return false;

            long localSize = new FileInfo(task.LocalPath).Length;
            return remoteSize == localSize;
        }
        catch
        {
            return false;
        }
    }

    private string BuildFtpUri(SyncTask task)
    {
        var cred = task.Credential;
        int port = cred.Port > 0 ? cred.Port : 21;
        string remotePath = task.RemoteRelativePath.TrimStart('/');
        return $"ftp://{cred.ServerName}:{port}/{remotePath}";
    }

    private System.Net.FtpWebRequest CreateRequest(
        SyncTask task, string uri, string method)
    {
        var request = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(uri);
        request.Credentials = new System.Net.NetworkCredential(
            task.Credential.UserName, task.Credential.Password);
        request.Method = method;
        request.UseBinary = true;
        request.UsePassive = true;
        request.Timeout = 30000;
        return request;
    }
}
