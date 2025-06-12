using ClassCompass.Shared.Models;
using ClassCompass.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassCompass.Shared.Services
{
    public class GradesApi
    {
        private readonly AppDbContext _context;

        public GradesApi(AppDbContext context)
        {
            _context = context;
        }

        // Assign a new grade
        public async Task<Grade?> AssignGradeAsync(Grade grade)
        {
            try
            {
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                return grade;
            }
            catch
            {
                return null;
            }
        }

        // Get grades by student ID
        public async Task<IEnumerable<Grade>> GetGradesByStudentAsync(int studentId)
        {
            return await _context.Grades
                .Where(g => g.StudentId == studentId)
                .OrderByDescending(g => g.DateRecorded)
                .ToListAsync();
        }

        // Get grades by subject
        public async Task<IEnumerable<Grade>> GetGradesBySubjectAsync(string subject)
        {
            return await _context.Grades
                .Where(g => g.Subject == subject)
                .OrderByDescending(g => g.DateRecorded)
                .ToListAsync();
        }

        // Get grades by classroom ID
        public async Task<IEnumerable<Grade>> GetGradesByClassroomAsync(int classroomId)
        {
            return await _context.Grades
                .Where(g => g.ClassRoomId == classroomId)
                .OrderByDescending(g => g.DateRecorded)
                .ToListAsync();
        }

        // Get grade by ID
        public async Task<Grade?> GetGradeByIdAsync(int gradeId)
        {
            return await _context.Grades
                .FirstOrDefaultAsync(g => g.Id == gradeId);
        }

        // Update grade
        public async Task<bool> UpdateGradeAsync(Grade grade)
        {
            try
            {
                _context.Grades.Update(grade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Delete grade
        public async Task<bool> DeleteGradeAsync(int gradeId)
        {
            try
            {
                var grade = await _context.Grades.FindAsync(gradeId);
                if (grade == null) return false;

                _context.Grades.Remove(grade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Get student grade average
        public async Task<double> GetStudentGradeAverageAsync(int studentId)
        {
            var grades = await _context.Grades
                .Where(g => g.StudentId == studentId && g.Score.HasValue && !g.IsExcused)
                .ToListAsync();

            if (!grades.Any()) return 0;

            return Math.Round(grades.Average(g => g.Percentage), 2);
        }

        // Get student subject average
        public async Task<double> GetStudentSubjectAverageAsync(int studentId, string subject)
        {
            var grades = await _context.Grades
                .Where(g => g.StudentId == studentId && g.Subject == subject && g.Score.HasValue && !g.IsExcused)
                .ToListAsync();

            if (!grades.Any()) return 0;

            return Math.Round(grades.Average(g => g.Percentage), 2);
        }
    }
}