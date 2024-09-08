using Kvizazov.Model;
using Kvizazov.Repositories;
using Kvizazov.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for QuizSearch.xaml
    /// </summary>
    public partial class QuizSearch : Window
    {
        QuizRepository quizRepository = new QuizRepository();
        TeamRepository teamRepository = new TeamRepository();
        UserRepository userRepository = new UserRepository();

        List<Team> myPairs, myTeams = new List<Team>();

        public QuizSearch()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbType.ItemsSource = Enum.GetValues(typeof(QuizType));
            cmbPair.IsEnabled = false;
            cmbTeam.IsEnabled = false;
            if (!UserSessionService.Instance.IsLoggedInUserAdmin())
            {
                btnNewQuiz.IsEnabled = false;
                btnNewQuiz.Background = Brushes.Gray;
            }
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

            List<Quiz> quizzesBeforeUpdate = await quizRepository.GetAllQuizzesOfType((QuizType)cmbType.SelectedItem);

            List<Quiz> quizzes;
            if(quizzesBeforeUpdate.Where(_quiz => _quiz != null).ToList().Count != 0)
            {
                quizzes = await quizRepository.UpdateQuizStatus(quizzesBeforeUpdate.Where(_quiz => _quiz != null).ToList());
            } else
            {
                quizzes = quizzesBeforeUpdate;
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
            dgQuizes.Columns[8].Header = "Status";
            dgQuizes.Columns[9].Visibility = Visibility.Hidden;
        }

        private async void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            if (dgQuizes.SelectedItem == null)
            {
                MessageBox.Show("Odaberite kviz na koji se želite prijaviti");
            } else if (dgQuizes.SelectedItem is Quiz selectedQuiz)
            {
                if(selectedQuiz.Type == QuizType.Individualni)
                {
                    if(selectedQuiz.LeaderboardSolo.Exists(user => user.Key.Username == UserSessionService.Instance.LoggedInUser.Username))
                    {
                        MessageBox.Show("Već ste prijavljeni na ovaj kviz");
                        (sender as Button).Focusable = false;
                        this.Focus();
                        return;
                    }
                    selectedQuiz.LeaderboardSolo.Add(new KeyValuePair<User, float>(UserSessionService.Instance.LoggedInUser, 0));
                    UserSessionService.Instance.LoggedInUser.SignedUpQuizzes.Add(selectedQuiz.Id);
                    if (UserSessionService.Instance.LoggedInUser.SignedUpQuizzes.Contains(0))
                    {
                       UserSessionService.Instance.LoggedInUser.SignedUpQuizzes.Remove(0);
                    }
                    await userRepository.CreateOrUpdateUser(UserSessionService.Instance.LoggedInUser);
                    if(selectedQuiz.LeaderboardSolo.Exists(user => user.Key.Username == "default"))
                    {
                        selectedQuiz.LeaderboardSolo.RemoveAt(0);
                    }
                } else if(selectedQuiz.Type == QuizType.Parovi)
                {
                    if(selectedQuiz.LeaderboardPairTeam.Exists(team => team.Key.Name == cmbPair.SelectedItem.ToString()))
                    {
                        MessageBox.Show("Već ste prijavljeni na ovaj kviz");
                        (sender as Button).Focusable = false;
                        this.Focus();
                        return;
                    }
                    Team selectedPair = await teamRepository.GetTeamByName(cmbPair.SelectedItem.ToString());
                    selectedQuiz.LeaderboardPairTeam.Add(new KeyValuePair<Team, float>(selectedPair, 0));
                    selectedPair.SignedUpQuizzes.Add(selectedQuiz.Id);
                    if (selectedPair.SignedUpQuizzes.Contains(0))
                    {
                        selectedPair.SignedUpQuizzes.Remove(0);
                    }
                    await teamRepository.CreateOrUpdateTeam(selectedPair);
                    if(selectedQuiz.LeaderboardPairTeam.Exists(team => team.Key.Name == "default"))
                    {
                        selectedQuiz.LeaderboardPairTeam.RemoveAt(0);
                    }
                } else
                {
                    if(selectedQuiz.LeaderboardPairTeam.Exists(team => team.Key.Name == cmbTeam.SelectedItem.ToString()))
                    {
                        MessageBox.Show("Već ste prijavljeni na ovaj kviz");
                        (sender as Button).Focusable = false;
                        this.Focus();
                        return;
                    }
                    Team selectedTeam = await teamRepository.GetTeamByName(cmbTeam.SelectedItem.ToString());
                    selectedQuiz.LeaderboardPairTeam.Add(new KeyValuePair<Team, float>(selectedTeam, 0));
                    selectedTeam.SignedUpQuizzes.Add(selectedQuiz.Id);
                    if (selectedTeam.SignedUpQuizzes.Contains(0))
                    {
                        selectedTeam.SignedUpQuizzes.Remove(0);
                    }
                    await teamRepository.CreateOrUpdateTeam(selectedTeam);
                    if (selectedQuiz.LeaderboardPairTeam.Exists(team => team.Key.Name == "default"))
                    {
                        selectedQuiz.LeaderboardPairTeam.RemoveAt(0);
                    }
                }
                await quizRepository.CreateOrUpdateQuiz(selectedQuiz);
                MessageBox.Show("Uspješno ste se prijavili na kviz");
                (sender as Button).Focusable = false;
                this.Focus();
            }
        }

        private void btnNewQuiz_Click(object sender, RoutedEventArgs e)
        {
            NewQuiz newQuiz = new NewQuiz();
            newQuiz.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void cmbPair_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgQuizes.ItemsSource = null;
        }

        private void cmbTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgQuizes.ItemsSource = null;
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
    }
}
