using FileSync.Models.Models;
using FileSync.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileSync.Web.Controllers;

public class SyncTaskController : Controller
{
    private readonly ISyncTaskService _syncTaskService;
    private readonly ICredentialService _credentialService;

    public SyncTaskController(
        ISyncTaskService syncTaskService,
        ICredentialService credentialService)
    {
        _syncTaskService = syncTaskService;
        _credentialService = credentialService;
    }

    public async Task<IActionResult> Index()
    {
        var tasks = await _syncTaskService.GetAllAsync();
        return View(tasks);
    }

    public async Task<IActionResult> Create()
    {
        var credentials = await _credentialService.GetAllAsync();

        var vm = new SyncTaskViewModel
        {
            ScheduleTime = DateTime.Now,
            Credentials = credentials.Select(c => new SelectListItem
            {
                Text = FormatCredentialLabel(c),
                Value = c.Id.ToString()
            })
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SyncTaskViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var credentials = await _credentialService.GetAllAsync();

            vm.Credentials = credentials.Select(c => new SelectListItem
            {
                Text = FormatCredentialLabel(c),
                Value = c.Id.ToString()
            });

            return View(vm);
        }

        var task = new SyncTask
        {
            TaskName = vm.TaskName,
            CredentialId = vm.CredentialId,
            RemoteRelativePath = vm.RemoteRelativePath,
            LocalPath = vm.LocalPath,
            SkipIfExists = vm.SkipIfExists,
            ResumeIfInterrupted = vm.ResumeInterruptedCopy,
            VerifyAfterCopy = vm.VerifyAfterCopy,
            IsEnabled = vm.IsEnabled,
            Schedule = new Schedule
            {
                StartTime = vm.ScheduleTime,
                IsEnabled = vm.IsEnabled
            }
        };

        await _syncTaskService.CreateAsync(task);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var task = await _syncTaskService.GetByIdAsync(id);

        if (task == null)
            return NotFound();

        var credentials = await _credentialService.GetAllAsync();

        var vm = new SyncTaskViewModel
        {
            Id = task.Id,
            TaskName = task.TaskName,
            CredentialId = task.CredentialId,
            RemoteRelativePath = task.RemoteRelativePath,
            LocalPath = task.LocalPath,
            ScheduleTime = task.Schedule?.StartTime ?? DateTime.Now,
            SkipIfExists = task.SkipIfExists,
            ResumeInterruptedCopy = task.ResumeIfInterrupted,
            VerifyAfterCopy = task.VerifyAfterCopy,
            IsEnabled = task.IsEnabled,

            Credentials = credentials.Select(c => new SelectListItem
            {
                Text = FormatCredentialLabel(c),
                Value = c.Id.ToString()
            })
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SyncTaskViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var credentials = await _credentialService.GetAllAsync();
            vm.Credentials = credentials.Select(c => new SelectListItem
            {
                Text = FormatCredentialLabel(c),
                Value = c.Id.ToString()
            });
            return View(vm);
        }

        var task = await _syncTaskService.GetByIdAsync(vm.Id);
        if (task == null)
            return NotFound();

        task.TaskName = vm.TaskName;
        task.CredentialId = vm.CredentialId;
        task.RemoteRelativePath = vm.RemoteRelativePath;
        task.LocalPath = vm.LocalPath;
        task.SkipIfExists = vm.SkipIfExists;
        task.ResumeIfInterrupted = vm.ResumeInterruptedCopy;
        task.VerifyAfterCopy = vm.VerifyAfterCopy;
        task.IsEnabled = vm.IsEnabled;

        if (task.Schedule != null)
        {
            task.Schedule.StartTime = vm.ScheduleTime;
        }
        else
        {
            task.Schedule = new Schedule
            {
                StartTime = vm.ScheduleTime,
                IsEnabled = vm.IsEnabled
            };
        }

        await _syncTaskService.UpdateAsync(task);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var task = await _syncTaskService.GetByIdAsync(id);

        if (task == null)
            return NotFound();

        return View(task);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _syncTaskService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Toggle(int id)
    {
        var task = await _syncTaskService.GetByIdAsync(id);

        if (task == null)
            return NotFound();

        task.IsEnabled = !task.IsEnabled;

        await _syncTaskService.UpdateAsync(task);

        return RedirectToAction(nameof(Index));
    }

    private static string FormatCredentialLabel(Credential c)
    {
        if (c.Protocol == ProtocolType.FTP)
        {
            string port = c.Port > 0 ? $":{c.Port}" : "";
            string name = !string.IsNullOrEmpty(c.ConnectionName)
                ? $"{c.ConnectionName} — " : "";
            return $"{name}FTP: {c.ServerName}{port}";
        }
        else
        {
            string name = !string.IsNullOrEmpty(c.ConnectionName)
                ? $"{c.ConnectionName} — " : "";
            return $"{name}SMB: \\\\{c.ServerName}\\{c.ShareName}";
        }
    }
}