using ClassCompass.Shared.Services.HttpClientServices;
using Microsoft.Maui.Controls;
using ClassCompass.Shared.Models;

namespace ClassCompassApp.Views
{
    public partial class TeacherGradingPage : ContentPage
    {
        private readonly IGradesHttpService _gradesHttpService;

        public TeacherGradingPage(IGradesHttpService gradesHttpService)
        {
            InitializeComponent();
            _gradesHttpService = gradesHttpService;
        }

        private async void OnSubmitGradeClicked(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(ScoreEntry.Text, out int score))
                {
                    await DisplayAlert("Error", "Please enter a valid numeric score.", "OK");
                    return;
                }

                if (!int.TryParse(StudentIdEntry.Text, out int studentId))
                {
                    await DisplayAlert("Error", "Please enter a valid numeric student ID.", "OK");
                    return;
                }

                var grade = new Grade
                {
                    StudentId = studentId,
                    Score = score,
                    DateRecorded = DateTime.Now
                    // Add other properties if needed, e.g., AssignmentId, etc.
                };

                // Use HTTP service instead of direct database access
                var result = await _gradesHttpService.CreateGradeAsync(grade);

                if (result != null)
                {
                    await DisplayAlert("Success", "Grade submitted successfully!", "OK");

                    // Clear the form
                    ScoreEntry.Text = string.Empty;
                    StudentIdEntry.Text = string.Empty;
                }
                else
                {
                    await DisplayAlert("Error", "Failed to submit grade.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to submit grade: {ex.Message}", "OK");
            }
        }
    }
}
