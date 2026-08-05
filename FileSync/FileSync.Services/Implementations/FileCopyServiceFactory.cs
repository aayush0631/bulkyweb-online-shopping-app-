using FileSync.Models.Models;
using FileSync.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace FileSync.Services.Implementations;

public class FileCopyServiceFactory : IFileCopyServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FileCopyServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IFileCopyService Create(ProtocolType protocol)
    {
        return protocol switch
        {
            ProtocolType.FTP => _serviceProvider.GetRequiredService<FtpFileCopyService>(),
            _ => _serviceProvider.GetRequiredService<FileCopyService>()
        };
    }
}
