using System;
using System.Threading.Tasks;
using ClassCompass.Shared.Services.Security;

namespace ClassCompassApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("ClassCompass Application Starting...");
            
            // Create encryption service
            var encryptionService = new EncryptionService();
            
            // Test encryption/decryption
            try
            {
                var testString = "Hello, ClassCompass!";
                var encrypted = await encryptionService.EncryptStringAsync(testString);
                var decrypted = await encryptionService.DecryptStringAsync(encrypted);
                
                Console.WriteLine($"Original: {testString}");
                Console.WriteLine($"Encrypted: {encrypted}");
                Console.WriteLine($"Decrypted: {decrypted}");
                Console.WriteLine($"Success: {testString == decrypted}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Encryption test failed: {ex.Message}");
            }
            
            Console.WriteLine("Program completed successfully.");
        }
    }
}

