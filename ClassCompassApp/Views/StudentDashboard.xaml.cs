using System;
using Microsoft.Maui.Controls;
using ClassCompass.Shared.Services.HttpClientServices;
namespace ClassCompassApp.Views
{
    public partial class StudentDashboard : ContentPage
    {
        public StudentDashboard()
        {
            InitializeComponent();
        }

        private async void OnHomeworkClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomeworkPage());
        }

        private async void OnGradesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GradesPage());
        }

        private async void OnStartDayClicked(object sender, EventArgs e)
        {
            // You can add logic here to trigger notifications or reminders
            await DisplayAlert("Reminder", "Your day has started! Notifications enabled.", "OK");
        }
    }
}


