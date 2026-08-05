namespace FileSync.Models.Models
{
    public class Credential
    {
        public int Id { get; set; }
        public string ConnectionName { get; set; } = string.Empty;
        public ProtocolType Protocol { get; set; } = ProtocolType.SMB;
        public string ServerName { get; set; } = string.Empty;
        public int Port { get; set; } = 0;
        public string ShareName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
