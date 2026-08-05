using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using SftpFileSync.Worker.Models;
using System.Net.Mail;

namespace SftpFileSync.Worker.Services;

public class SftpClientService : ISftpClientService, IDisposable
{
    // Logger instance.
    private readonly ILogger<SftpClientService> _logger;

    // Tracks whether this service has already been disposed.
    private bool _disposed;
    private SftpClient? _sftpClient;

    public SftpClientService(
        ILogger<SftpClientService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Throws an exception if the service has already been disposed.
    /// </summary>
    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(SftpClientService));
    }

    /// <summary>
    /// Opens an SFTP connection.
    /// </summary>
    public async Task CreateAsync(SftpSettings settings)
    {
        ThrowIfDisposed();

        if (_sftpClient != null && _sftpClient.IsConnected)
            return;

        _sftpClient = new SftpClient(
            settings.Host,
            settings.Port,
            settings.Username,
            settings.Password);

        try
        {
            var client = _sftpClient;

            // SSH.NET Connect() is synchronous.
            await Task.Run(() => client!.Connect());

            _logger.LogInformation(
                "Connected to SFTP server {Host}",
                settings.Host);
        }
        catch (Exception ex)
        {
            _sftpClient?.Dispose();
            _sftpClient = null;

            _logger.LogError(
                ex,
                "Failed to connect to SFTP server {Host}",
                settings.Host);

            throw;
        }
    }

    /// <summary>
    /// Closes the SFTP connection.
    /// </summary>
    public void Disconnect()
    {
        ThrowIfDisposed();

        if (_sftpClient == null)
            return;

        try
        {
            if (_sftpClient.IsConnected)
            {
                _sftpClient.Disconnect();

                _logger.LogInformation(
                    "Disconnected from SFTP server.");
            }
        }
        finally
        {
            _sftpClient.Dispose();
            _sftpClient = null;
        }
    }

    /// <summary>
    /// Downloads a file from the SFTP server.
    /// </summary>
    public Task DownloadFileAsync(
        string remoteFile,
        string localPath)
    {
        ThrowIfDisposed();

        if (_sftpClient == null || !_sftpClient.IsConnected)
            throw new InvalidOperationException(
                "SFTP client is not connected.");

        var directory = Path.GetDirectoryName(localPath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using FileStream stream = File.Create(localPath);

        _sftpClient.DownloadFile(remoteFile, stream);

        _logger.LogInformation(
            "Downloaded {RemoteFile} -> {LocalFile}",
            remoteFile,
            localPath);

        return Task.CompletedTask;
    }

    // Recursively traverses the remote directory tree and collects all files.
    public List<RemoteFileInfo> GetFilesRecursive(string remotePath)
    {
        var result = new List<RemoteFileInfo>();

        // Get all files and folders in the current directory.
        var entries = _sftpClient.ListDirectory(remotePath);

        foreach (var entry in entries)
        {
            // Skip the special "." and ".." directory entries.
            if (entry.Name == "." || entry.Name == "..")
                continue;

            if (entry.IsDirectory)
            {
                // Explore subdirectories recursively.
                result.AddRange(GetFilesRecursive(entry.FullName));
            }
            else
            {
                // Store metadata for each discovered file.
                result.Add(new RemoteFileInfo
                {
                    FullName = entry.FullName,
                    Name = entry.Name,
                    Size = entry.Length,
                    ModifiedDate = entry.LastWriteTime,
                    isDirectory = false
                });
            }
        }

        return result;
    }

    /// <summary>
    /// Lists all files inside the specified directory recursively.
    /// </summary>
    public Task<List<RemoteFileInfo>> ListFileAsync(string remotePath)
    {
        ThrowIfDisposed();

        if (_sftpClient == null || !_sftpClient.IsConnected)
            throw new InvalidOperationException(
                "SFTP client is not connected.");

        if (string.IsNullOrWhiteSpace(remotePath))
            throw new ArgumentException(
                "Remote path cannot be empty.",
                nameof(remotePath));

        var files = GetFilesRecursive(remotePath);

        return Task.FromResult(files);
    }

    /// <summary>
    /// Releases all unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        try
        {
            if (_sftpClient != null)
            {
                if (_sftpClient.IsConnected)
                {
                    _sftpClient.Disconnect();
                }

                _sftpClient.Dispose();
            }
        }
        finally
        {
            _sftpClient = null;
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}