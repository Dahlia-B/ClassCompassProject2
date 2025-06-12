using System.Collections.Generic;
using System.Threading.Tasks;
using ClassCompassApi.Shared.Models;

namespace ClassCompassApi.Interfaces
{
    public interface IScheduleService
    {
        Task<ScheduleEntry> GenerateWeeklySchedule(ScheduleEntry request);
        Task UpdateClassSchedule(ScheduleTemplate update);
        Task<ScheduleEntry> GetClassSchedule(string classId);
        Task<ScheduleEntry> GetTeacherSchedule(string teacherId);
    }

}

