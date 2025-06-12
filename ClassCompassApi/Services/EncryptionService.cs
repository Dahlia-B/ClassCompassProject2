using System.Security.Cryptography;
using System.Text;

namespace ClassCompassApi.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class EncryptionService : IEncryptionService
    {
        private readonly string _encryptionKey;

        public EncryptionService(IConfiguration configuration)
        {
            _encryptionKey = configuration["EncryptionKey"] ?? "MySecretKey123456789012345678901234567890"; // 32 chars
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using var swEncrypt = new StreamWriter(csEncrypt);

            swEncrypt.Write(plainText);
            csEncrypt.FlushFinalBlock();

            var iv = aes.IV;
            var encrypted = msEncrypt.ToArray();
            var result = new byte[iv.Length + encrypted.Length];

            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - 16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, 16);
            Buffer.BlockCopy(fullCipher, 16, cipher, 0, cipher.Length);

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var msDecrypt = new MemoryStream(cipher);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
