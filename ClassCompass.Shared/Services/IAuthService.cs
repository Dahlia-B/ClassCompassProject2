using ClassCompass.Shared.Models;
using System.Threading.Tasks;

namespace ClassCompass.Shared.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with username and password
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>User object if authentication succeeds, null if it fails</returns>
        Task<User?> AuthenticateAsync(string username, string password);

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="user">The user to register</param>
        /// <returns>True if registration succeeds, false if it fails</returns>
        Task<bool> RegisterUserAsync(User user);

        /// <summary>
        /// Gets a user by their ID
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>User object if found, null if not found</returns>
        Task<User?> GetUserByIdAsync(int userId);

        /// <summary>
        /// Gets a user by username
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>User object if found, null if not found</returns>
        Task<User?> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Validates if a username is available for registration
        /// </summary>
        /// <param name="username">The username to check</param>
        /// <returns>True if username is available, false if already taken</returns>
        Task<bool> IsUsernameAvailableAsync(string username);

        /// <summary>
        /// Updates user information
        /// </summary>
        /// <param name="user">The user with updated information</param>
        /// <returns>True if update succeeds, false if it fails</returns>
        Task<bool> UpdateUserAsync(User user);

        /// <summary>
        /// Deactivates a user account (soft delete)
        /// </summary>
        /// <param name="userId">The user ID to deactivate</param>
        /// <returns>True if deactivation succeeds, false if it fails</returns>
        Task<bool> DeactivateUserAsync(int userId);

        /// <summary>
        /// Changes user password (if passwords are stored in related tables)
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="currentPassword">The current password</param>
        /// <param name="newPassword">The new password</param>
        /// <returns>True if password change succeeds, false if it fails</returns>
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

        /// <summary>
        /// Gets users by role
        /// </summary>
        /// <param name="role">The role to filter by</param>
        /// <returns>List of users with the specified role</returns>
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);

        /// <summary>
        /// Creates a user account linked to a teacher
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="teacherId">The teacher ID to link</param>
        /// <returns>Created user if successful, null if failed</returns>
        Task<User?> CreateTeacherUserAsync(string username, int teacherId);

        /// <summary>
        /// Creates a user account linked to a student
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="studentId">The student ID to link</param>
        /// <returns>Created user if successful, null if failed</returns>
        Task<User?> CreateStudentUserAsync(string username, int studentId);

        /// <summary>
        /// Gets a user by their linked teacher ID
        /// </summary>
        /// <param name="teacherId">The teacher ID</param>
        /// <returns>User object if found, null if not found</returns>
        Task<User?> GetUserByTeacherIdAsync(int teacherId);

        /// <summary>
        /// Gets a user by their linked student ID
        /// </summary>
        /// <param name="studentId">The student ID</param>
        /// <returns>User object if found, null if not found</returns>
        Task<User?> GetUserByStudentIdAsync(int studentId);
    }
}