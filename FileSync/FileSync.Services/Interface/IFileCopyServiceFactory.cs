using FileSync.Models.Models;

namespace FileSync.Services.Interface;

public interface IFileCopyServiceFactory
{
    IFileCopyService Create(ProtocolType protocol);
}
