using FileSync.Models.Models;

namespace FileSync.Services.Interface;

public interface IConnectionServiceFactory
{
    INetworkConnectionService Create(ProtocolType protocol);
}
