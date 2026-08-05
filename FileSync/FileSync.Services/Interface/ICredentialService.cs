using FileSync.Models.Models;

public interface ICredentialService
{
    Task CreateAsync(Credential credential);
    Task UpdateAsync(Credential credential);
    Task<Credential> GetByIdAsync(int id);
    Task DeleteAsync(int? id);
    Task<IEnumerable<Credential>> GetAllAsync();
    bool Validate(Credential credential);
}