using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Services.Interface
{
    public interface IEncryptionService
    {
        string Encrypt(string password);
        string Decrypt(string encryptedPassword);
    }
}
