using FileSync.Models.Models;
using FileSync.Services.Helper;
using FileSync.Utilities;

public class NetworkConnectionService : INetworkConnectionService
{
    private string? _connectedShare;
    public Task<bool> ConnectAsync(Credential credential)
    {
        string remotePath = $@"\\{credential.ServerName}\{credential.ShareName}";
        var resurce = new NETRESOURCE
        {
            dwType=SD.ResourceTypeDisk, // RESOURCETYPE_DISK
            lpRemoteName= remotePath
        };

        int result = NativeMethods.WNetAddConnection2(
            resurce, 
            credential.Password, 
            credential.UserName, 
            0);
        if(result == 0)
        {
            _connectedShare = remotePath;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task DisconnectAsync()
    {
        if(!string.IsNullOrEmpty(_connectedShare))
        {
            NativeMethods.WNetCancelConnection2(_connectedShare, 0, true);
            _connectedShare=null;
        }

        return Task.CompletedTask;
    }
}