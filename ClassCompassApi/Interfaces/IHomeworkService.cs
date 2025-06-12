using System.Threading.Tasks;
using ClassCompassApi.Shared.Models;

namespace ClassCompassApi.Interfaces
{
    public interface IHomeworkService
    {
        Task AssignHomework(Assignment assignment);
        Task GradeHomework(Grade grade);
    }
}

