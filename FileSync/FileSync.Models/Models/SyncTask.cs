using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace FileSync.Models.Models
{
    public class SyncTask
    {
        public int Id { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string RemoteRelativePath { get; set; } = string.Empty;
        public string LocalPath { get; set; } = string.Empty;
        public bool ResumeIfInterrupted { get; set; } = true;
        public bool SkipIfExists { get; set; } = true;
        public bool VerifyAfterCopy { get; set; } = true;
        public bool IsEnabled { get; set; } = true;
        public int CredentialId { get; set; }
        [ForeignKey(nameof(ScheduleId))]
        public Credential Credential { get; set; } = null!;
        public int ScheduleId { get; set; }
        [ForeignKey(nameof(ScheduleId))]    
        public Schedule Schedule { get; set; } = null!;
    }
}
