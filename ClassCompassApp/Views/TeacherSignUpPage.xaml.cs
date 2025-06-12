using ClassCompass.Shared.Services;
using ClassCompass.Shared.Models;
using Microsoft.Maui.Controls;

namespace ClassCompassApp.Views
{
    public partial class TeacherSignUpPage : ContentPage
    {
        private readonly ApiService _apiService;

        public TeacherSignUpPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnRegisterTeacherClicked(object sender, EventArgs e)
        {
            var registerButton = sender as Button;
            if (registerButton != null)
            {
                registerButton.IsEnabled = false;
                registerButton.Text = "Registering...";
            }

            try
            {
                // Validate input fields
                if (string.IsNullOrWhiteSpace(TeacherNameEntry.Text) ||
                    string.IsNullOrWhiteSpace(SubjectEntry.Text))
                {
                    await DisplayAlert("Error", "Please fill in Teacher Name and Subject", "OK");
                    return;
                }

                int schoolId = 1; // Default school ID
                if (!string.IsNullOrWhiteSpace(SchoolIdEntry.Text))
                {
                    if (!int.TryParse(SchoolIdEntry.Text, out schoolId))
                    {
                        await DisplayAlert("Error", "School ID must be numeric", "OK");
                        return;
                    }
                }

                // Create teacher registration request
                var teacherRequest = new TeacherRegistrationRequest
                {
                    Name = TeacherNameEntry.Text.Trim(),
                    Subject = SubjectEntry.Text.Trim(),
                    SchoolId = schoolId,
                    PasswordHash = string.IsNullOrWhiteSpace(PasswordEntry.Text) ?
                                  "teacher123" : PasswordEntry.Text
                };

                // Call the API
                var response = await _apiService.CreateTeacherAsync(teacherRequest);

                if (response?.Success == true)
                {
                    await DisplayAlert("Success",
                        $"Teacher registered successfully!\n\n" +
                        $"????? Name: {teacherRequest.Name}\n" +
                        $"?? Subject: {teacherRequest.Subject}\n" +
                        $"?? Teacher ID: {response.TeacherId}\n" +
                        $"?? School ID: {schoolId}\n\n" +
                        $"? Registration completed!",
                        "OK");

                    // Clear the form
                    TeacherNameEntry.Text = string.Empty;
                    if (TeacherIdEntry != null) TeacherIdEntry.Text = string.Empty;
                    if (PasswordEntry != null) PasswordEntry.Text = string.Empty;
                    SubjectEntry.Text = string.Empty;
                    if (SchoolIdEntry != null) SchoolIdEntry.Text = string.Empty;

                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await DisplayAlert("Error",
                        response?.Message ?? "Failed to register teacher. Please try again.",
                        "OK");
                }
            }
            catch (HttpRequestException httpEx)
            {
                await DisplayAlert("Network Error",
                    $"Could not connect to server.\n\nDetails: {httpEx.Message}",
                    "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
            }
            finally
            {
                // Reset button - using different variable name to avoid conflict
                if (registerButton != null)
                {
                    registerButton.IsEnabled = true;
                    registerButton.Text = "Register Teacher";
                }
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
