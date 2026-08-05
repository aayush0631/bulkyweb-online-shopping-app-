using System.ComponentModel.DataAnnotations;
using FileSync.Models.Models;

namespace FileSync.Models.ViewModels
{
    public class CredentialViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Connection Name")]
        public string ConnectionName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Protocol")]
        public ProtocolType Protocol { get; set; } = ProtocolType.SMB;

        [Required]
        [Display(Name = "Server Name / IP")]
        public string ServerName { get; set; } = string.Empty;

        [Display(Name = "Port")]
        public int Port { get; set; } = 0;

        [Display(Name = "Share Name")]
        public string ShareName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
    }
}
