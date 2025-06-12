using ClassCompass.Shared.Models;
using ClassCompass.Shared.Services;
using Microsoft.Maui.Controls;
using ClassCompass.Shared.Services.HttpClientServices;
namespace ClassCompassApp.Views
{
    public partial class HomeworkSubmissionPage : ContentPage
    {
        private string SelectedFileURL = string.Empty;
        private readonly HomeworkApi _homeworkApi;

        public HomeworkSubmissionPage(HomeworkApi homeworkApi)
        {
            InitializeComponent();
            _homeworkApi = homeworkApi;
        }

        private async void OnChooseFileClicked(object sender, EventArgs e)
        {
            // TODO: Replace with actual file picker logic
            SelectedFileURL = "https://example.com/dummyfile.pdf"; // Mock value
            await DisplayAlert("File Selected", "Mock file selected!", "OK");
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            // Check current user
            var currentUser = App.CurrentUser;
            if (currentUser == null || string.IsNullOrWhiteSpace(currentUser.UserId))
            {
                await DisplayAlert("Error", "No current user found or user ID is missing.", "OK");
                return;
            }

            // Parse student ID
            if (!int.TryParse(currentUser.UserId, out int studentId))
            {
                await DisplayAlert("Error", "Invalid user ID format.", "OK");
                return;
            }

            // Validate Homework ID entry
            if (string.IsNullOrEmpty(HomeworkIdEntry?.Text) || !int.TryParse(HomeworkIdEntry.Text, out int homeworkId))
            {
                await DisplayAlert("Error", "Please enter a valid numeric Homework ID.", "OK");
                return;
            }

            // Validate file selection
            if (string.IsNullOrEmpty(SelectedFileURL))
            {
                await DisplayAlert("Error", "Please choose a file before submitting.", "OK");
                return;
            }

            // Create submission model
            var submission = new HomeworkSubmission
            {
                HomeworkId = homeworkId,
                StudentId = studentId,
                FileURL = SelectedFileURL,
                SubmittedDate = DateTime.Now
            };

            // Attempt submission
            try
            {
                bool result = await _homeworkApi.SubmitHomework(submission);
                if (result)
                {
                    await DisplayAlert("Success", "Homework submitted successfully!", "OK");
                    HomeworkIdEntry.Text = string.Empty;
                    SelectedFileURL = string.Empty;
                }
                else
                {
                    await DisplayAlert("Error", "Homework submission failed.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to submit homework: {ex.Message}", "OK");
            }
        }
    }
}

