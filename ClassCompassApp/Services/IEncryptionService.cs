using System;

namespace ClassCompassApp.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string plaintext);
        string Decrypt(string ciphertext);
    }
}





