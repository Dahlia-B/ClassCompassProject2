using ClassCompass.Shared.Services;
using ClassCompass.Shared.Models;
using Microsoft.Maui.Controls;

namespace ClassCompassApp.Views
{
    public partial class StudentSignUpPage : ContentPage
    {
        private readonly ApiService _apiService;

        public StudentSignUpPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnRegisterStudentClicked(object sender, EventArgs e)
        {
            var registerButton = sender as Button;
            if (registerButton != null)
            {
                registerButton.IsEnabled = false;
                registerButton.Text = "Registering...";
            }

            try
            {
                // For now, let's create a simple student registration
                // You can add more fields to your XAML and update this accordingly

                var studentRequest = new StudentRegistrationRequest
                {
                    Name = "New Student", // You'll need to add StudentNameEntry to your XAML
                    ClassName = "General Class",
                    TeacherId = 1, // Default teacher
                    ClassId = 1, // Default class
                    PasswordHash = "student123"
                };

                // Call the API
                var response = await _apiService.CreateStudentAsync(studentRequest);

                if (response?.Success == true)
                {
                    await DisplayAlert("Success",
                        $"Student registered successfully!\n\n" +
                        $"????? Name: {studentRequest.Name}\n" +
                        $"?? Class: {studentRequest.ClassName}\n" +
                        $"?? Student ID: {response.StudentId}\n\n" +
                        $"? Registration completed!",
                        "OK");

                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await DisplayAlert("Error",
                        response?.Message ?? "Failed to register student. Please try again.",
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
                    registerButton.Text = "Register Student";
                }
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
