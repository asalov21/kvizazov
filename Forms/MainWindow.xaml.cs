using Kvizazov.Services;
using System.Windows;
using System.Windows.Media;

namespace Kvizazov.Forms
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void btnTeamManagement_Click(object sender, RoutedEventArgs e)
        {
            TeamManagement teamManagement = new TeamManagement();
            teamManagement.Show();
            this.Close();
        }

        private void btnQuizSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPlayQuiz_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLeaderboards_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNewQuestions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            Profile profile = new Profile();
            profile.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserSessionService.Instance.IsUserLoggedIn())
            {
                btnLogin.Visibility = Visibility.Hidden;
                btnProfile.Visibility = Visibility.Visible;
                btnTeamManagement.IsEnabled = true;
                btnTeamManagement.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"));
                btnQuizSearch.IsEnabled = true;
                btnQuizSearch.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"));
                btnPlayQuiz.IsEnabled = true;
                btnPlayQuiz.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"));
                btnLeaderboards.IsEnabled = true;
                btnLeaderboards.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"));
                btnNewQuestions.IsEnabled = true;
                btnNewQuestions.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"));
            } else
            {
                btnTeamManagement.IsEnabled = false;
                btnTeamManagement.Background = Brushes.Gray;
                btnQuizSearch.IsEnabled = false;
                btnQuizSearch.Background = Brushes.Gray;
                btnPlayQuiz.IsEnabled = false;
                btnPlayQuiz.Background = Brushes.Gray;
                btnLeaderboards.IsEnabled = false;
                btnLeaderboards.Background = Brushes.Gray;
                btnNewQuestions.IsEnabled = false;
                btnNewQuestions.Background = Brushes.Gray;
            }
        }
    }
}
