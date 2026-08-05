using FileSync.Models.Models;
using FileSync.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace FileSync.Services.Implementations;

public class ConnectionServiceFactory : IConnectionServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ConnectionServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public INetworkConnectionService Create(ProtocolType protocol)
    {
        return protocol switch
        {
            ProtocolType.FTP => _serviceProvider.GetRequiredService<FtpConnectionService>(),
            _ => _serviceProvider.GetRequiredService<NetworkConnectionService>()
        };
    }
}
