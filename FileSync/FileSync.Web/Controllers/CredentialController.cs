using FileSync.Models.Models;
using FileSync.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FileSync.Web.Controllers;

public class CredentialController : Controller
{
    private readonly ICredentialService _credentialService;

    public CredentialController(
        ICredentialService credentialService)
    {
        _credentialService = credentialService;
    }

    public async Task<IActionResult> Index()
    {
        var credentials = await _credentialService.GetAllAsync();
        var vm = credentials.Select(c => new CredentialViewModel
        {
            Id = c.Id,
            ConnectionName = c.ConnectionName,
            Protocol = c.Protocol,
            ServerName = c.ServerName,
            Port = c.Port,
            ShareName = c.ShareName,
            UserName = c.UserName,
            Password = c.Password
        }).ToList();
        return View(vm);
    }

    public IActionResult Create()
    {
        return View(new CredentialViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CredentialViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var credential = new Credential
        {
            ConnectionName = vm.ConnectionName,
            Protocol = vm.Protocol,
            ServerName = vm.ServerName,
            Port = vm.Port,
            ShareName = vm.ShareName,
            UserName = vm.UserName,
            Password = vm.Password
        };

        await _credentialService.CreateAsync(credential);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var credential = await _credentialService.GetByIdAsync(id);

        if (credential == null)
            return NotFound();

        var vm = new CredentialViewModel
        {
            Id = credential.Id,
            ConnectionName = credential.ConnectionName,
            Protocol = credential.Protocol,
            ServerName = credential.ServerName,
            Port = credential.Port,
            ShareName = credential.ShareName,
            UserName = credential.UserName,
            Password = credential.Password
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CredentialViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var credential = new Credential
        {
            Id = vm.Id,
            ConnectionName = vm.ConnectionName,
            Protocol = vm.Protocol,
            ServerName = vm.ServerName,
            Port = vm.Port,
            ShareName = vm.ShareName,
            UserName = vm.UserName,
            Password = vm.Password
        };

        await _credentialService.UpdateAsync(credential);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var credential = await _credentialService.GetByIdAsync(id);

        if (credential == null)
            return NotFound();

        return View(credential);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _credentialService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}
