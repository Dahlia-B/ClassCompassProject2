using ClassCompass.Shared.Services.HttpClientServices;
using ClassCompass.Shared.Services;
using ClassCompass.Shared.Models;
using Microsoft.Maui.Controls;
namespace ClassCompassApp.Views;

public partial class AttendanceTrackingPage : ContentPage
{
    private readonly AttendanceApi _attendanceApi;

    public AttendanceTrackingPage(AttendanceApi attendanceApi)
    {
        InitializeComponent();
        _attendanceApi = attendanceApi;
    }

    private async void OnSubmitAttendanceClicked(object sender, EventArgs e)
    {
        var studentIdText = StudentIdEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(studentIdText))
        {
            await DisplayAlert("Error", "Please enter a Student ID", "OK");
            return;
        }

        if (!int.TryParse(studentIdText, out int studentId))
        {
            await DisplayAlert("Error", "Student ID must be a valid number", "OK");
            return;
        }

        if (StatusPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select a status", "OK");
            return;
        }

        bool isPresent = StatusPicker.SelectedItem.ToString()!.Equals("Present", StringComparison.OrdinalIgnoreCase);

        var attendanceRecord = new Attendance
        {
            Date = DateTime.Now,
            Present = isPresent   
        };

        try
        {
            await _attendanceApi.MarkAttendance(studentId.ToString(), attendanceRecord);

            await DisplayAlert("Success", "Attendance recorded successfully!", "OK");

            StudentIdEntry.Text = string.Empty;
            StatusPicker.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to record attendance: {ex.Message}", "OK");
        }
    }
}

