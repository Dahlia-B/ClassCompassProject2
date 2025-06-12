using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassCompass.Shared.Services.Security
{
    public interface IEncryptionService
    {
        Task<string> EncryptStringAsync(string plainText, string? key = null);
        Task<string> DecryptStringAsync(string cipherText, string? key = null);
        string EncryptString(string plainText, string? key = null);
        string DecryptString(string cipherText, string? key = null);
        Task<byte[]> EncryptBytesAsync(byte[] plainBytes, string? key = null);
        Task<byte[]> DecryptBytesAsync(byte[] cipherBytes, string? key = null);
        string GenerateKey();
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class EncryptionService : IEncryptionService
    {
        private readonly string _defaultKey;
        private const int KeySize = 256;
        private const int BlockSize = 128;

        public EncryptionService()
        {
            _defaultKey = GenerateKey();
        }

        public EncryptionService(string defaultKey)
        {
            _defaultKey = defaultKey;
        }

        public async Task<string> EncryptStringAsync(string plainText, string? key = null)
        {
            return await Task.Run(() => EncryptString(plainText, key));
        }

        public async Task<string> DecryptStringAsync(string cipherText, string? key = null)
        {
            return await Task.Run(() => DecryptString(cipherText, key));
        }

        public string EncryptString(string plainText, string? key = null)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            var keyToUse = key ?? _defaultKey;
            var keyBytes = Encoding.UTF8.GetBytes(keyToUse);
            
            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            
            // Use first 32 bytes of key for AES-256
            var aesKey = new byte[32];
            Array.Copy(keyBytes, aesKey, Math.Min(keyBytes.Length, 32));
            aes.Key = aesKey;
            
            aes.GenerateIV();
            
            using var encryptor = aes.CreateEncryptor();
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using var swEncrypt = new StreamWriter(csEncrypt);
            
            swEncrypt.Write(plainText);
            swEncrypt.Close();
            
            var encrypted = msEncrypt.ToArray();
            var result = new byte[aes.IV.Length + encrypted.Length];
            Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
            Array.Copy(encrypted, 0, result, aes.IV.Length, encrypted.Length);
            
            return Convert.ToBase64String(result);
        }

        public string DecryptString(string cipherText, string? key = null)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            var keyToUse = key ?? _defaultKey;
            var keyBytes = Encoding.UTF8.GetBytes(keyToUse);
            var cipherBytes = Convert.FromBase64String(cipherText);
            
            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            
            // Use first 32 bytes of key for AES-256
            var aesKey = new byte[32];
            Array.Copy(keyBytes, aesKey, Math.Min(keyBytes.Length, 32));
            aes.Key = aesKey;
            
            // Extract IV from cipher bytes
            var iv = new byte[16];
            var encrypted = new byte[cipherBytes.Length - 16];
            Array.Copy(cipherBytes, 0, iv, 0, 16);
            Array.Copy(cipherBytes, 16, encrypted, 0, encrypted.Length);
            aes.IV = iv;
            
            using var decryptor = aes.CreateDecryptor();
            using var msDecrypt = new MemoryStream(encrypted);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            
            return srDecrypt.ReadToEnd();
        }

        public async Task<byte[]> EncryptBytesAsync(byte[] plainBytes, string? key = null)
        {
            return await Task.Run(() =>
            {
                var plainText = Convert.ToBase64String(plainBytes);
                var encrypted = EncryptString(plainText, key);
                return Convert.FromBase64String(encrypted);
            });
        }

        public async Task<byte[]> DecryptBytesAsync(byte[] cipherBytes, string? key = null)
        {
            return await Task.Run(() =>
            {
                var cipherText = Convert.ToBase64String(cipherBytes);
                var decrypted = DecryptString(cipherText, key);
                return Convert.FromBase64String(decrypted);
            });
        }

        public string GenerateKey()
        {
            using var rng = RandomNumberGenerator.Create();
            var keyBytes = new byte[32]; // 256-bit key
            rng.GetBytes(keyBytes);
            return Convert.ToBase64String(keyBytes);
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}
