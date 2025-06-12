using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassCompassApi.Shared.Data;
using ClassCompassApi.Shared.Models;

namespace ClassCompassApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SchoolsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterSchool([FromBody] School school)
        {
            try
            {
                _context.Schools.Add(school);
                await _context.SaveChangesAsync();
                return Ok(new { 
                    success = true,
                    message = "School registered successfully", 
                    school = school 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    success = false,
                    error = ex.Message 
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchools()
        {
            try
            {
                var schools = await _context.Schools.ToListAsync();
                return Ok(new {
                    success = true,
                    count = schools.Count,
                    schools = schools
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    success = false,
                    error = ex.Message 
                });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetSchool(int id)
        {
            try
            {
                var school = await _context.Schools.FindAsync(id);
                if (school == null) 
                    return NotFound(new { success = false, message = "School not found" });
                
                return Ok(new {
                    success = true,
                    school = school
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    success = false,
                    error = ex.Message 
                });
            }
        }
    }
}
