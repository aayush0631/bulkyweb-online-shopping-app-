using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileSync.Models.ViewModels;

public class SyncTaskViewModel
{
    public int Id { get; set; }
    [Required]
    public string TaskName { get; set; } = string.Empty;
    [Required]
    public int CredentialId { get; set; }
    [Required]
    public string RemoteRelativePath { get; set; } = string.Empty;
    [Required]
    public string LocalPath { get; set; } = string.Empty;
    public bool SkipIfExists { get; set; }
    public bool ResumeInterruptedCopy { get; set; }
    public bool VerifyAfterCopy { get; set; }
    public bool IsEnabled { get; set; } = true;
    [Required]
    public DateTime ScheduleTime { get; set; }
    public IEnumerable<SelectListItem> Credentials { get; set; }
        = Enumerable.Empty<SelectListItem>();
}