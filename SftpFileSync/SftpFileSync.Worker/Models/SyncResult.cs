using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SftpFileSync.Worker.Models
{
    public class SyncResult
    {
        public int DownloadFiles {  get; set; }
        public int SkippedFiles { get; set; }
        public int FailedFiles { get; set; }
        public List<string> Error { get; set; }
    }
}
