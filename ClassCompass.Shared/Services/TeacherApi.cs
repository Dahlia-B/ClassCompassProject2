using ClassCompass.Shared.Data;
using ClassCompass.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassCompass.Shared.Services
{
    public class TeacherApi
    {
        // Add constructor
        private readonly AppDbContext _dbContext;

        // Add constructor
        public TeacherApi(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Teacher?> AddTeacher(Teacher teacher)
        {
            _dbContext.Teachers.Add(teacher);
            var saved = await _dbContext.SaveChangesAsync();
            if (saved > 0)
                return teacher;
            return null;
        }




    }
}
