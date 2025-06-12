using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using ClassCompass.Shared.Services;

namespace ClassCompassApp.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiService _apiService;

        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnStudentLoginClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("StudentLoginPage");
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
                await Shell.Current.GoToAsync("TeacherLoginPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void OnSchoolSignUpClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("SchoolSignUpPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void OnTeacherSignUpClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("TeacherSignUpPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void OnStudentSignUpClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("StudentSignUpPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("LoginPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void OnTestApiClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await _apiService.TestConnectionAsync();
                await DisplayAlert("API Test", $"?? API Connection Test: {result}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("API Test", $"? API Connection Failed: {ex.Message}", "OK");
            }
        }

        private async void StudentButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("StudentLoginPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void TeacherButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("TeacherLoginPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Error: {ex.Message}", "OK");
            }
        }
    }
}

