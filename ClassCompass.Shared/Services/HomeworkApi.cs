using ClassCompass.Shared.Data;
using ClassCompass.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassCompass.Shared.Services
{
    public class HomeworkApi
    {
        private readonly AppDbContext _context;

        public HomeworkApi(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Assignment>> GetAssignmentsAsync()
        {
            return await _context.Assignments
                .Where(a => a.IsActive)
                .OrderBy(a => a.DueDate)
                .ToListAsync();
        }

        public async Task<Assignment> SubmitAssignmentAsync(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<bool> SubmitHomework(HomeworkSubmission submission)
        {
            _context.HomeworkSubmissions.Add(submission);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}