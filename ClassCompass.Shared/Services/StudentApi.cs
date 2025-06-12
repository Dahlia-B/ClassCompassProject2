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
    public class StudentApi
    {
        // Add constructor
        private readonly AppDbContext _dbContext;

        // Add constructor
        public StudentApi(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Student?> AddStudent(Student student)
        {
            _dbContext.Students.Add(student);
            var saved = await _dbContext.SaveChangesAsync();
            if (saved > 0)
                return student;
            return null;
        }




    }
}
