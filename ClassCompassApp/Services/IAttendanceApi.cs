using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClassCompassApp.Services
{
    public interface IAttendanceApi
    {
        Task<List<AttendanceRecord>> GetAttendanceAsync();
        Task<bool> MarkAttendanceAsync(AttendanceRecord record);
        Task<bool> UpdateAttendanceAsync(AttendanceRecord record);
    }

    public class AttendanceRecord
    {
        public string Id { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}





