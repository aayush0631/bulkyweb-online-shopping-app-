namespace FileSync.Models.Models
{
    public class CopyHistory
    {
        public int id { get; set; }
        public int SyncTaskId { get; set; }
        public SyncTask SyncTask { get; set; } = null!;
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public long BytesCopied { get; set; }
    }
}
