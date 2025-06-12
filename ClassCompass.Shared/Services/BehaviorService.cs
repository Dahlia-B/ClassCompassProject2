using ClassCompass.Shared.Models;
using ClassCompass.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassCompass.Shared.Services
{
    public class BehaviorService
    {
        private readonly AppDbContext _context;

        public BehaviorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RecordBehaviorAsync(BehaviorRemark remark)
        {
            try
            {
                _context.BehaviorRemarks.Add(remark);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<BehaviorRemark>> GetBehaviorRecordsByStudentAsync(int studentId)
        {
            try
            {
                return await _context.BehaviorRemarks
                    .Where(br => br.StudentId == studentId)
                    .OrderByDescending(br => br.Date)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<BehaviorRemark>();
            }
        }

        public async Task<List<BehaviorRemark>> GetBehaviorRecordsByClassroomAsync(int classroomId)
        {
            try
            {
                return await _context.BehaviorRemarks
                    .Include(br => br.Student)
                    .Where(br => br.Student != null && br.Student.ClassId == classroomId)
                    .OrderByDescending(br => br.Date)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<BehaviorRemark>();
            }
        }

        public async Task<BehaviorRemark?> GetBehaviorRecordByIdAsync(int id)
        {
            try
            {
                return await _context.BehaviorRemarks
                    .FirstOrDefaultAsync(br => br.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateBehaviorRecordAsync(BehaviorRemark remark)
        {
            try
            {
                _context.BehaviorRemarks.Update(remark);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteBehaviorRecordAsync(int id)
        {
            try
            {
                var remark = await _context.BehaviorRemarks.FindAsync(id);
                if (remark != null)
                {
                    _context.BehaviorRemarks.Remove(remark);
                    var result = await _context.SaveChangesAsync();
                    return result > 0;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}