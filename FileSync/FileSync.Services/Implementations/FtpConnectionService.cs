using FileSync.Models.Models;
using Microsoft.Extensions.Logging;

namespace FileSync.Services.Implementations;

public class FtpConnectionService : INetworkConnectionService
{
    private readonly ILogger<FtpConnectionService> _logger;

    public FtpConnectionService(ILogger<FtpConnectionService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> ConnectAsync(Credential credential)
    {
        try
        {
            int port = credential.Port > 0 ? credential.Port : 21;
            string ftpUri = $"ftp://{credential.ServerName}:{port}/";

            var request = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(ftpUri);
            request.Credentials = new System.Net.NetworkCredential(
                credential.UserName, credential.Password);
            request.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
            request.Timeout = 10000;

            using var response = (System.Net.FtpWebResponse)await request.GetResponseAsync();

            _logger.LogInformation(
                "FTP connection successful to {Server}:{Port} — Status: {Status}",
                credential.ServerName, port, response.StatusDescription);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "FTP connection failed to {Server}:{Port}",
                credential.ServerName, credential.Port);

            return false;
        }
    }

    public Task DisconnectAsync()
    {
        // FTP is stateless per-request, nothing to disconnect
        return Task.CompletedTask;
    }
}
