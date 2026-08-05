using SftpFileSync.Worker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SftpFileSync.Worker.Services
{
    public interface ISftpClientService : IDisposable
    {
        Task CreateAsync(SftpSettings settings);
        Task<List<RemoteFileInfo>> ListFileAsync(string remotePath);
        Task DownloadFileAsync (string remoteFile, string localPath);
        void Disconnect();
        List<RemoteFileInfo> GetFilesRecursive(string remotePath);
    }
}
