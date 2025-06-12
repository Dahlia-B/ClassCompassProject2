using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassCompassApi.Shared.Data;
using ClassCompassApi.Shared.Models;

namespace ClassCompassApi.Controllers
{
    [ApiController]
    [Route("api/classrooms")]
    public class ClassRoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassRoomsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterClassRoom([FromBody] ClassRoomDto classRoomDto)
        {
            try
            {
                var classRoom = new ClassRoom
                {
                    ClassId = classRoomDto.ClassId,
                    Class = classRoomDto.Class ?? string.Empty,
                    RoomNumber = classRoomDto.RoomNumber,
                    Subject = classRoomDto.Subject,
                    Schedule = classRoomDto.Schedule,
                    Notes = classRoomDto.Notes,
                    Capacity = classRoomDto.Capacity,
                    SchoolId = classRoomDto.SchoolId,
                    TeacherId = classRoomDto.TeacherId,
                    CreatedDate = DateTime.Now
                };

                _context.ClassRooms.Add(classRoom);
                await _context.SaveChangesAsync();
                
                return Ok(new { 
                    success = true,
                    message = "ClassRoom registered successfully", 
                    classRoom = new {
                        classRoom.Id,
                        classRoom.ClassId,
                        classRoom.Class,
                        classRoom.RoomNumber,
                        classRoom.Subject,
                        classRoom.Schedule,
                        classRoom.Capacity,
                        classRoom.SchoolId,
                        classRoom.TeacherId
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
        public async Task<IActionResult> GetAllClassRooms()
        {
            try
            {
                var classRooms = await _context.ClassRooms.ToListAsync();
                return Ok(new {
                    success = true,
                    count = classRooms.Count,
                    classRooms = classRooms.Select(c => new {
                        c.Id,
                        c.ClassId,
                        c.Class,
                        c.RoomNumber,
                        c.Subject,
                        c.Schedule,
                        c.Capacity,
                        c.SchoolId,
                        c.TeacherId
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
    }

    public class ClassRoomDto
    {
        public int ClassId { get; set; }
        public string? Class { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int Capacity { get; set; } = 30;
        public int SchoolId { get; set; }
        public int? TeacherId { get; set; }
    }
}
