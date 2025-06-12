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
    public class SchoolApi
    {
        // Add constructor
        private readonly AppDbContext _dbContext;

        // Add constructor
        public SchoolApi(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<School?> AddSchool(School school)
        {
            _dbContext.Schools.Add(school);
            var saved = await _dbContext.SaveChangesAsync();
            if (saved > 0)
                return school;
            return null;
        }




    }
}
