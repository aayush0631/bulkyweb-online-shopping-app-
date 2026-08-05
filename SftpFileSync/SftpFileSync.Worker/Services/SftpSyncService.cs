using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SftpFileSync.Worker.Models;

namespace SftpFileSync.Worker.Services;

public class SftpSyncService : ISftpSyncService
{
    private readonly ILogger<SftpSyncService> _logger;
    private readonly ISftpClientService _client;
    private readonly SftpConfiguration _sftpConfiguration;

    public SftpSyncService(
        ILogger<SftpSyncService> logger,
        ISftpClientService client,
        IOptions<SftpConfiguration> options)
    {
        _logger = logger;
        _client = client;
        _sftpConfiguration = options.Value;
    }

    /// <summary>
    /// Determines whether the remote file needs to be downloaded.
    /// </summary>
    private bool NeedsDownload(RemoteFileInfo remoteFile, string localPath)
    {
        // Download if the local file does not exist.
        if (!File.Exists(localPath))
            return true;

        var localFile = new FileInfo(localPath);

        // Download if the file size is different.
        if (localFile.Length != remoteFile.Size)
            return true;

        // Download if the remote file is newer.
        if (localFile.LastWriteTime < remoteFile.ModifiedDate)
            return true;

        // Otherwise, the local file is already up to date.
        return false;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        int downloaded = 0;
        int skipped = 0;
        int failed = 0;

        var startTime = DateTime.UtcNow;

        _logger.LogInformation("Starting SFTP synchronization...");

        try
        {
            foreach (var settings in _sftpConfiguration.Sftp)
            {
                _logger.LogInformation(
                    "Synchronizing device: {Device}",
                    settings.Name);

                try
                {
                    await _client.CreateAsync(settings);

                    Directory.CreateDirectory(settings.LocalPath);

                    var remoteFiles =
                        await _client.ListFileAsync(settings.RemotePath);

                    foreach (var remoteFile in remoteFiles)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        try
                        {
                            var relativePath = remoteFile.FullName
                                .Replace(settings.RemotePath, "")
                                .TrimStart('/', '\\');

                            var localFilePath = Path.Combine(
                                settings.LocalPath,
                                relativePath.Replace('/', Path.DirectorySeparatorChar));

                            var directory =
                                Path.GetDirectoryName(localFilePath);

                            if (!string.IsNullOrWhiteSpace(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            if (NeedsDownload(remoteFile, localFilePath))
                            {
                                _logger.LogInformation(
                                    "Downloading {File}",
                                    remoteFile.FullName);

                                await _client.DownloadFileAsync(
                                    remoteFile.FullName,
                                    localFilePath);

                                downloaded++;
                            }
                            else
                            {
                                _logger.LogInformation(
                                    "Skipping {File}",
                                    remoteFile.FullName);

                                skipped++;
                            }
                        }
                        catch (Exception ex)
                        {
                            failed++;

                            _logger.LogError(
                                ex,
                                "Failed to synchronize {File}",
                                remoteFile.FullName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to synchronize device {Device}",
                        settings.Name);
                }
                finally
                {
                    _client.Disconnect();
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Synchronization cancelled.");
        }
        finally
        {
            var duration = DateTime.UtcNow - startTime;

            _logger.LogInformation(
                """
            Synchronization completed

            Downloaded : {Downloaded}
            Skipped    : {Skipped}
            Failed     : {Failed}
            Duration   : {Duration}
            """,
                downloaded,
                skipped,
                failed,
                duration.ToString(@"hh\:mm\:ss"));
        }
    }
}