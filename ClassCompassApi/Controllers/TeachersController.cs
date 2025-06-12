using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassCompassApi.Shared.Data;
using ClassCompassApi.Shared.Models;

namespace ClassCompassApi.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterTeacher([FromBody] TeacherRegistrationDto teacherDto)
        {
            try
            {
                var teacher = new Teacher
                {
                    TeacherId = teacherDto.TeacherId,
                    Name = teacherDto.Name,
                    PasswordHash = teacherDto.PasswordHash ?? string.Empty,
                    Subject = teacherDto.Subject,
                    SchoolId = teacherDto.SchoolId,
                    IsActive = teacherDto.IsActive ?? true
                };

                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();
                
                return Ok(new { 
                    success = true,
                    message = "Teacher registered successfully", 
                    teacher = new {
                        teacher.Id,
                        teacher.TeacherId,
                        teacher.Name,
                        teacher.Subject,
                        teacher.SchoolId,
                        teacher.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    success = false,
                    error = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeachers()
        {
            try
            {
                var teachers = await _context.Teachers.ToListAsync();
                return Ok(new {
                    success = true,
                    count = teachers.Count,
                    teachers = teachers.Select(t => new {
                        t.Id,
                        t.TeacherId,
                        t.Name,
                        t.Subject,
                        t.SchoolId,
                        t.IsActive
                    })
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            try
            {
                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher == null) 
                    return NotFound(new { success = false, message = "Teacher not found" });
                
                return Ok(new {
                    success = true,
                    teacher = new {
                        teacher.Id,
                        teacher.TeacherId,
                        teacher.Name,
                        teacher.Subject,
                        teacher.SchoolId,
                        teacher.IsActive
                    }
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

    public class TeacherRegistrationDto
    {
        public int TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }
        public string Subject { get; set; } = string.Empty;
        public int SchoolId { get; set; }
        public bool? IsActive { get; set; }
    }
}
