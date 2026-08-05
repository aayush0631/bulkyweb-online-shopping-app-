using FileSync.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Services.Implementations
{
    public class EncryptionService : IEncryptionService
    {
        public string Encrypt(string password)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(password);

            byte[] encryptedBytes = ProtectedData.Protect(
                plainBytes,
                null,
                DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string encryptedPassword)
        {
            byte[] encryptedBytes =
                Convert.FromBase64String(encryptedPassword);

            byte[] plainBytes =
                ProtectedData.Unprotect(
                    encryptedBytes,
                    null,
                    DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
