namespace ClassCompassApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public enum UserRole
    {
        Student,
        Teacher,
        Administrator
    }

    public class AttendanceRecord
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class Homework
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class HomeworkSubmission
    {
        public int Id { get; set; }
        public int HomeworkId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string SubmissionText { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; }
        public double? Grade { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public List<string> AttachmentUrls { get; set; } = new List<string>();
    }

    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string AssignmentName { get; set; } = string.Empty;
        public double Score { get; set; }
        public double MaxScore { get; set; }
        public double Percentage => MaxScore > 0 ? (Score / MaxScore) * 100 : 0;
        public string LetterGrade { get; set; } = string.Empty;
        public DateTime DateGraded { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
    }

    public class GradeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Weight { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

