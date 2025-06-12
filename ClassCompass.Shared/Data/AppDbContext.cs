using Microsoft.EntityFrameworkCore;
using ClassCompass.Shared.Models;

namespace ClassCompass.Shared.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Core entities that we know exist and work
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        
        // Additional entities - only add DbSets if the model files actually exist
        public DbSet<Grade> Grades { get; set; }
        public DbSet<BehaviorRemark> BehaviorRemarks { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<HomeworkSubmission> HomeworkSubmissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Student relationships
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Teacher)
                .WithMany(t => t.Students)
                .HasForeignKey(s => s.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.ClassRoom)
                .WithMany()
                .HasForeignKey(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Teacher relationships  
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.School)
                .WithMany(s => s.Teachers)
                .HasForeignKey(t => t.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassRoom relationships
            modelBuilder.Entity<ClassRoom>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassRoom>()
                .HasOne(c => c.School)
                .WithMany()
                .HasForeignKey(c => c.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            // Attendance relationships
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure other relationships with minimal assumptions
            try
            {
                // Only configure if models exist and have expected properties
                if (typeof(Grade).GetProperty("StudentId") != null)
                {
                    modelBuilder.Entity<Grade>()
                        .HasOne<Student>()
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict);
                }

                if (typeof(BehaviorRemark).GetProperty("StudentId") != null)
                {
                    modelBuilder.Entity<BehaviorRemark>()
                        .HasOne<Student>()
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict);
                }

                if (typeof(Assignment).GetProperty("TeacherId") != null)
                {
                    modelBuilder.Entity<Assignment>()
                        .HasOne<Teacher>()
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Restrict);
                }
            }
            catch
            {
                // Ignore configuration errors for now
            }
        }
    }
}
