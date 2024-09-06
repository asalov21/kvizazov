using Kvizazov.Model;
using Kvizazov.Repositories;
using Kvizazov.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kvizazov.Forms
{
    /// <summary>
    /// Interaction logic for PlayQuiz.xaml
    /// </summary>
    public partial class PlayQuiz : Window
    {
        QuizRepository quizRepository = new QuizRepository();
        TeamRepository teamRepository = new TeamRepository();
        UserRepository userRepository = new UserRepository();

        List<Team> myPairs, myTeams = new List<Team>();
        public PlayQuiz()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbType.ItemsSource = Enum.GetValues(typeof(QuizType));
            cmbPair.IsEnabled = false;
            cmbTeam.IsEnabled = false;

            myPairs = await teamRepository.CaptainTeamsOfSpecificType(TeamType.Par, UserSessionService.Instance.LoggedInUser);
            myTeams = await teamRepository.CaptainTeamsOfSpecificType(TeamType.Tim, UserSessionService.Instance.LoggedInUser);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgQuizes.ItemsSource = null;
            if ((QuizType)cmbType.SelectedItem == QuizType.Individualni)
            {
                cmbPair.IsEnabled = false;
                cmbPair.SelectedItem = null;
                cmbTeam.IsEnabled = false;
                cmbTeam.SelectedItem = null;
            } else if ((QuizType)cmbType.SelectedItem == QuizType.Parovi)
            {
                cmbPair.IsEnabled = true;
                cmbTeam.IsEnabled = false;
                cmbTeam.SelectedItem = null;

                cmbPair.ItemsSource = myPairs.Select(team => team.Name);
            } else
            {
                cmbPair.IsEnabled = false;
                cmbPair.SelectedItem = null;
                cmbTeam.IsEnabled = true;

                cmbTeam.ItemsSource = myTeams.Select(team => team.Name);
            }
        }

        private void cmbPair_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgQuizes.ItemsSource = null;
        }

        private void cmbTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgQuizes.ItemsSource = null;
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (cmbType.SelectedItem == null)
            {
                MessageBox.Show("Morate odabrati vrstu kviza");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            } else if ((QuizType)cmbType.SelectedItem == QuizType.Parovi && cmbPair.SelectedItem == null)
            {
                MessageBox.Show("Morate odabrati par");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            } else if ((QuizType)cmbType.SelectedItem == QuizType.Timski && cmbTeam.SelectedItem == null)
            {
                MessageBox.Show("Morate odabrati tim");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            }

            List<Quiz> quizzes;

            if((QuizType)cmbType.SelectedItem == QuizType.Individualni)
            {
                quizzes = await quizRepository.GetAllOpenQuizzesUserSignedUp(UserSessionService.Instance.LoggedInUser);
            }else if((QuizType)cmbType.SelectedItem == QuizType.Parovi)
            {
                quizzes = await quizRepository.GetAllOpenQuizzesTeamSignedUp(cmbPair.SelectedItem.ToString());
            } else
            {
                quizzes = await quizRepository.GetAllOpenQuizzesTeamSignedUp(cmbTeam.SelectedItem.ToString());
            }

            dgQuizes.ItemsSource = quizzes;

            (sender as Button).Focusable = false;
            this.Focus();

            dgQuizes.Columns[0].Visibility = Visibility.Hidden;
            dgQuizes.Columns[1].Header = "Vrsta";
            dgQuizes.Columns[2].Header = "Početak";
            dgQuizes.Columns[3].Header = "Završetak";
            dgQuizes.Columns[4].Header = "Broj pitanja";
            dgQuizes.Columns[5].Header = "Sekundi po pitanju";
            dgQuizes.Columns[6].Visibility = Visibility.Hidden;
            dgQuizes.Columns[7].Visibility = Visibility.Hidden;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
