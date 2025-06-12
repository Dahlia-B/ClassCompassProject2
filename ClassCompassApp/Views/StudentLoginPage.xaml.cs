using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace ClassCompassApp.Views
{
    public partial class StudentLoginPage : ContentPage
    {
        public StudentLoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("StudentDashboard");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void OnStudentLoginClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("StudentDashboard");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("StudentDashboard");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }
    }
}

