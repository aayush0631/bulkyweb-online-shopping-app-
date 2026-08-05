using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SftpFileSync.Worker.Models
{
    public class RemoteFileInfo
    {
        public string FullName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool isDirectory { get; set; }
    }
}
