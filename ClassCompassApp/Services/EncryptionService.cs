using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassCompassApp.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string _key;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public EncryptionService()
        {
            _key = "YourSecretKey123";
        }

        public string Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                return string.Empty;

            _semaphore.Wait();
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = GetKeyBytes(_key);
                    aes.GenerateIV();

                    using (var encryptor = aes.CreateEncryptor())
                    {
                        var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
                        var encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
                        
                        var result = new byte[aes.IV.Length + encryptedBytes.Length];
                        Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
                        Array.Copy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);
                        
                        return Convert.ToBase64String(result);
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public string Decrypt(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return string.Empty;

            _semaphore.Wait();
            try
            {
                var ciphertextBytes = Convert.FromBase64String(ciphertext);

                using (var aes = Aes.Create())
                {
                    aes.Key = GetKeyBytes(_key);
                    
                    var iv = new byte[aes.IV.Length];
                    var encryptedData = new byte[ciphertextBytes.Length - iv.Length];
                    
                    Array.Copy(ciphertextBytes, 0, iv, 0, iv.Length);
                    Array.Copy(ciphertextBytes, iv.Length, encryptedData, 0, encryptedData.Length);
                    
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        var decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<string> EncryptAsync(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                return string.Empty;

            await _semaphore.WaitAsync();
            try
            {
                return await Task.Run(() => Encrypt(plaintext));
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<string> DecryptAsync(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return string.Empty;

            await _semaphore.WaitAsync();
            try
            {
                return await Task.Run(() => Decrypt(ciphertext));
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private byte[] GetKeyBytes(string key)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }
    }
}





