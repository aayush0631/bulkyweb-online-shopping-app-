using FileSync.Models.Models;

public interface INetworkConnectionService
{
    Task<bool> ConnectAsync(Credential credential);

    Task DisconnectAsync();
}