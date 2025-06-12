using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassCompass.Shared.Data;
using ClassCompass.Shared.Models;

namespace ClassCompassApi.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterStudent([FromBody] StudentRegistrationDto studentDto)
        {
            try
            {
                // Create student with the actual model structure
                var student = new Student
                {
                    StudentId = studentDto.StudentId,
                    Name = studentDto.Name,
                    PasswordHash = studentDto.PasswordHash ?? string.Empty,
                    ClassName = studentDto.ClassName ?? string.Empty,
                    TeacherId = studentDto.TeacherId,
                    ClassId = studentDto.ClassId,
                    EnrollmentDate = studentDto.EnrollmentDate ?? DateTime.Now,
                    IsActive = studentDto.IsActive ?? true,
                    NotificationsEnabled = studentDto.NotificationsEnabled ?? true
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Student registered successfully",
                    student = new
                    {
                        student.Id,
                        student.StudentId,
                        student.Name,
                        student.ClassName,
                        student.TeacherId,
                        student.ClassId,
                        student.EnrollmentDate,
                        student.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _context.Students.ToListAsync();
                return Ok(new
                {
                    success = true,
                    count = students.Count,
                    students = students.Select(s => new {
                        s.Id,
                        s.StudentId,
                        s.Name,
                        s.ClassName,
                        s.TeacherId,
                        s.ClassId,
                        s.EnrollmentDate,
                        s.IsActive
                    })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                    return NotFound(new { success = false, message = "Student not found" });

                return Ok(new
                {
                    success = true,
                    student = new
                    {
                        student.Id,
                        student.StudentId,
                        student.Name,
                        student.ClassName,
                        student.TeacherId,
                        student.ClassId,
                        student.EnrollmentDate,
                        student.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }
    }

    // DTO for student registration
    public class StudentRegistrationDto
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }
        public string? ClassName { get; set; }
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? NotificationsEnabled { get; set; }
    }
}