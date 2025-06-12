using Microsoft.Maui.Controls;

namespace ClassCompassApp.Views;

public partial class TeacherDashboardPage : ContentPage
{
    public TeacherDashboardPage()
    {
        InitializeComponent();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}

