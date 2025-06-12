using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace ClassCompassApp.Views
{
    public partial class TeacherLoginPage : ContentPage
    {
        public TeacherLoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("TeacherDashboard");
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

        private async void OnTeacherLoginClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("TeacherDashboard");
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
                await Shell.Current.GoToAsync("TeacherDashboard");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }
    }
}

