using FileSync.DataAccess.Repository.IRepository;
using FileSync.Models.Models;
using FileSync.Services.Interface;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using System.Text;

namespace FileSync.Services.Implementations;

public class CredentialService : ICredentialService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEncryptionService _encryptionService;
    public CredentialService(IUnitOfWork UnitOfWork, IEncryptionService EncryptionService)
    {
        _encryptionService = EncryptionService;
        _unitOfWork = UnitOfWork;
    }

    public bool Validate(Credential credential)
    {
        return !string.IsNullOrWhiteSpace(credential.ServerName)
            && !string.IsNullOrWhiteSpace(credential.UserName)
            && !string.IsNullOrWhiteSpace(credential.Password);
    }

    public async Task CreateAsync(Credential credential)
    {
        if (!Validate(credential))
            throw new ArgumentException("Invalid credential.");

        credential.Password = _encryptionService.Encrypt(credential.Password);
        _unitOfWork.Credential.Add(credential);
        _unitOfWork.Save();
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(Credential credential)
    {
        if (!Validate(credential))
            throw new ArgumentException("Invalid credential.");

        credential.Password = _encryptionService.Encrypt(credential.Password);

        _unitOfWork.Credential.Update(credential);
        _unitOfWork.Save();

        await Task.CompletedTask;
    }

    public Task<Credential> GetByIdAsync(int id)
    {
        var credential = _unitOfWork.Credential.Get(c => c.Id == id);
        if (credential == null) 
        {
            throw new KeyNotFoundException($"Credential with ID {id} not found.");
        }
        return Task.FromResult(credential);
    }

    public async Task DeleteAsync(int? id)
    {
        if(id == null || id <= 0)
        {
            throw new ArgumentException("Invalid credential ID.");
        }
        var credential = _unitOfWork.Credential.Get(c => c.Id == id);
        if(credential == null)
        {
            throw new KeyNotFoundException("Credential not found.");
        }
        _unitOfWork.Credential.Remove(credential);
        _unitOfWork.Save();
        await Task.CompletedTask;
    }

    public Task<IEnumerable<Credential>> GetAllAsync()
    {
        var credentials = _unitOfWork.Credential.GetAll();
        return Task.FromResult(credentials);
    }
}