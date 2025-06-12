using ClassCompass.Shared.Models;
using ClassCompass.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassCompass.Shared.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _connectionString;

        public AuthService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty", nameof(username));
            
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            try
            {
                await Task.Delay(100);

                return username switch
                {
                    "admin" when password == "password123" => new User { UserId = 1, Username = username, Role = "Administrator" },
                    "teacher1" when password == "teacher123" => new User { UserId = 2, Username = username, Role = "Teacher", TeacherId = 1 },
                    "student1" when password == "student123" => new User { UserId = 3, Username = username, Role = "Student", StudentId = 1 },
                    _ => null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication error: {ex.Message}");
                throw new InvalidOperationException("Authentication failed due to an internal error", ex);
            }
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Username)) throw new ArgumentException("Username is required", nameof(user));
            if (string.IsNullOrWhiteSpace(user.Role)) throw new ArgumentException("Role is required", nameof(user));

            try
            {
                await Task.Delay(150);
                var isAvailable = await IsUsernameAvailableAsync(user.Username);
                if (!isAvailable) return false;
                Console.WriteLine($"Mock: Registering user {user.Username} with role {user.Role}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                throw new InvalidOperationException("User registration failed due to an internal error", ex);
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("User ID must be greater than zero", nameof(userId));

            try
            {
                await Task.Delay(50);
                return userId switch
                {
                    1 => new User { UserId = 1, Username = "admin", Role = "Administrator" },
                    2 => new User { UserId = 2, Username = "teacher1", Role = "Teacher", TeacherId = 1 },
                    3 => new User { UserId = 3, Username = "student1", Role = "Student", StudentId = 1 },
                    _ => null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get user error: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve user due to an internal error", ex);
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be null or empty", nameof(username));

            try
            {
                await Task.Delay(50);
                return username switch
                {
                    "admin" => new User { UserId = 1, Username = "admin", Role = "Administrator" },
                    "teacher1" => new User { UserId = 2, Username = "teacher1", Role = "Teacher", TeacherId = 1 },
                    "student1" => new User { UserId = 3, Username = "student1", Role = "Student", StudentId = 1 },
                    _ => null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get user by username error: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve user by username", ex);
            }
        }

        public async Task<bool> IsUsernameAvailableAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be null or empty", nameof(username));

            try
            {
                await Task.Delay(30);
                var existingUsernames = new[] { "admin", "teacher1", "student1" };
                return !existingUsernames.Contains(username, StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Username availability check error: {ex.Message}");
                throw new InvalidOperationException("Failed to check username availability", ex);
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.UserId <= 0) throw new ArgumentException("User ID must be greater than zero", nameof(user));

            try
            {
                await Task.Delay(100);
                Console.WriteLine($"Mock: Updating user {user.UserId} - {user.Username}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update user error: {ex.Message}");
                throw new InvalidOperationException("Failed to update user", ex);
            }
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("User ID must be greater than zero", nameof(userId));

            try
            {
                await Task.Delay(80);
                Console.WriteLine($"Mock: Deactivating user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deactivate user error: {ex.Message}");
                throw new InvalidOperationException("Failed to deactivate user", ex);
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            if (userId <= 0) throw new ArgumentException("User ID must be greater than zero", nameof(userId));
            if (string.IsNullOrWhiteSpace(currentPassword)) throw new ArgumentException("Current password cannot be null or empty", nameof(currentPassword));
            if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentException("New password cannot be null or empty", nameof(newPassword));

            try
            {
                await Task.Delay(120);
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;
                Console.WriteLine($"Mock: Changing password for user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Change password error: {ex.Message}");
                throw new InvalidOperationException("Failed to change password", ex);
            }
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role cannot be null or empty", nameof(role));

            try
            {
                await Task.Delay(80);
                var allUsers = new List<User>
                {
                    new User { UserId = 1, Username = "admin", Role = "Administrator" },
                    new User { UserId = 2, Username = "teacher1", Role = "Teacher", TeacherId = 1 },
                    new User { UserId = 3, Username = "student1", Role = "Student", StudentId = 1 },
                    new User { UserId = 4, Username = "teacher2", Role = "Teacher", TeacherId = 2 },
                    new User { UserId = 5, Username = "student2", Role = "Student", StudentId = 2 }
                };
                return allUsers.Where(u => u.Role.Equals(role, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get users by role error: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve users by role", ex);
            }
        }

        public async Task<User?> CreateTeacherUserAsync(string username, int teacherId)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required", nameof(username));
            if (teacherId <= 0) throw new ArgumentException("Teacher ID must be greater than zero", nameof(teacherId));

            var user = new User { Username = username, Role = "Teacher", TeacherId = teacherId };
            var success = await RegisterUserAsync(user);
            return success ? user : null;
        }

        public async Task<User?> CreateStudentUserAsync(string username, int studentId)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required", nameof(username));
            if (studentId <= 0) throw new ArgumentException("Student ID must be greater than zero", nameof(studentId));

            var user = new User { Username = username, Role = "Student", StudentId = studentId };
            var success = await RegisterUserAsync(user);
            return success ? user : null;
        }

        public async Task<User?> GetUserByTeacherIdAsync(int teacherId)
        {
            if (teacherId <= 0) throw new ArgumentException("Teacher ID must be greater than zero", nameof(teacherId));

            try
            {
                await Task.Delay(50);
                return teacherId switch
                {
                    1 => new User { UserId = 2, Username = "teacher1", Role = "Teacher", TeacherId = 1 },
                    2 => new User { UserId = 4, Username = "teacher2", Role = "Teacher", TeacherId = 2 },
                    _ => null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get user by teacher ID error: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve user by teacher ID", ex);
            }
        }

        public async Task<User?> GetUserByStudentIdAsync(int studentId)
        {
            if (studentId <= 0) throw new ArgumentException("Student ID must be greater than zero", nameof(studentId));

            try
            {
                await Task.Delay(50);
                return studentId switch
                {
                    1 => new User { UserId = 3, Username = "student1", Role = "Student", StudentId = 1 },
                    2 => new User { UserId = 5, Username = "student2", Role = "Student", StudentId = 2 },
                    _ => null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get user by student ID error: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve user by student ID", ex);
            }
        }
    }
}
