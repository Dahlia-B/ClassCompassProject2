using System.Threading.Tasks;
using ClassCompass.Shared.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClassCompass.Shared.Services
{
    public class NotificationService
    {
        public async Task SendClassReminder(string userId)
        {
            // Simulate async operation
            await Task.Delay(100);
            System.Diagnostics.Debug.WriteLine($"Class reminder sent to {userId}");
        }
    }
}
