using ClassCompass.Shared.Models;
using ClassCompass.Shared.Services;
using Microsoft.Maui.Controls;

namespace ClassCompassApp.Views
{
    public partial class SchoolSignUpPage : ContentPage
    {
        private readonly ApiService _apiService;

        // Parameterless constructor for Shell navigation
        public SchoolSignUpPage()
        {
            InitializeComponent();
            _apiService = new ApiService(); // Uses the fixed ApiService with correct URL
        }

        private async void OnRegisterSchoolClicked(object sender, EventArgs e)
        {
            try
            {
                // Validate input fields
                if (string.IsNullOrWhiteSpace(SchoolNameEntry.Text) ||
                    string.IsNullOrWhiteSpace(ClassCountEntry.Text))
                {
                    await DisplayAlert("Error", "Please fill in School Name and Number of Classes", "OK");
                    return;
                }

                if (!int.TryParse(ClassCountEntry.Text, out int classCount))
                {
                    await DisplayAlert("Error", "Number of classes must be numeric.", "OK");
                    return;
                }

                // Disable the button during processing
                if (sender is Button button)
                {
                    button.IsEnabled = false;
                    button.Text = "Registering...";
                }

                // Create the school registration request
                var schoolRequest = new SchoolRegistrationRequest
                {
                    Name = SchoolNameEntry.Text.Trim(),
                    NumberOfClasses = classCount,
                    Description = string.IsNullOrWhiteSpace(SchoolIdEntry.Text) ?
                                 "No description provided" : SchoolIdEntry.Text.Trim()
                };

                // Call the API
                var response = await _apiService.CreateSchoolAsync(schoolRequest);

                if (response?.Success == true)
                {
                    await DisplayAlert("Success",
                        $"School registered successfully!\n\n" +
                        $"?? School Name: {schoolRequest.Name}\n" +
                        $"?? School ID: {response.SchoolId}\n" +
                        $"?? Number of Classes: {classCount}\n\n" +
                        $"? Data saved to server successfully!",
                        "OK");

                    // Clear the form
                    SchoolNameEntry.Text = string.Empty;
                    SchoolIdEntry.Text = string.Empty;
                    ClassCountEntry.Text = string.Empty;

                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await DisplayAlert("Error",
                        response?.Message ?? "Failed to create school. Please try again.",
                        "OK");
                }
            }
            catch (HttpRequestException httpEx)
            {
                await DisplayAlert("Network Error",
                    $"Could not connect to server. Please check your internet connection.\n\nDetails: {httpEx.Message}",
                    "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",
                    $"Registration failed: {ex.Message}",
                    "OK");
            }
            finally
            {
                // Reset button
                if (sender is Button button)
                {
                    button.IsEnabled = true;
                    button.Text = "Register School";
                }
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
