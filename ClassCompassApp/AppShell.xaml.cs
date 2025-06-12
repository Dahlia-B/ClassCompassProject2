using ClassCompassApp.Views;
using Microsoft.Maui.Controls;

namespace ClassCompassApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register navigation routes with DI-compatible handlers
            // THESE ARE NEEDED - pages not defined in AppShell.xaml
            Routing.RegisterRoute(nameof(SchoolSignUpPage), typeof(SchoolSignUpPage));
            Routing.RegisterRoute(nameof(TeacherSignUpPage), typeof(TeacherSignUpPage));
            Routing.RegisterRoute(nameof(StudentSignUpPage), typeof(StudentSignUpPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(TeacherLoginPage), typeof(TeacherLoginPage));
            Routing.RegisterRoute(nameof(StudentLoginPage), typeof(StudentLoginPage));
            Routing.RegisterRoute(nameof(TeacherGradingPage), typeof(TeacherGradingPage));
            Routing.RegisterRoute(nameof(AttendanceTrackingPage), typeof(AttendanceTrackingPage));
            Routing.RegisterRoute(nameof(HomeworkSubmissionPage), typeof(HomeworkSubmissionPage));
            Routing.RegisterRoute(nameof(StudentDashboard), typeof(StudentDashboard));
            Routing.RegisterRoute(nameof(GradesPage), typeof(GradesPage));
            Routing.RegisterRoute(nameof(HomeworkPage), typeof(HomeworkPage));
            Routing.RegisterRoute(nameof(HomeworkAssignmentPage), typeof(HomeworkAssignmentPage));
            Routing.RegisterRoute(nameof(TeacherSchedulePage), typeof(TeacherSchedulePage));
        }
    }
}
