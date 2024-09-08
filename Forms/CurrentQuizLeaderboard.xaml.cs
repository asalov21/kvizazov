using Kvizazov.Model;
using Kvizazov.Repositories;
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
    /// Interaction logic for CurrentQuizLeaderboard.xaml
    /// </summary>
    public partial class CurrentQuizLeaderboard : Window
    {
        private Quiz quiz;

        public CurrentQuizLeaderboard(Quiz _quiz)
        {
            InitializeComponent();
            quiz = _quiz;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(quiz.Type == QuizType.Individualni)
            {
                dgResults.ItemsSource = quiz.LeaderboardSolo.OrderByDescending(item => item.Value);
                dgResults.Columns[0].Header = "Korisničko ime";
                var columnUser = dgResults.Columns[0] as DataGridTextColumn;
                var bindingUser = new Binding("Key.Username");
                columnUser.Binding = bindingUser;
            } else if(quiz.Type == QuizType.Parovi)
            {
                dgResults.ItemsSource = quiz.LeaderboardPairTeam.OrderByDescending(item => item.Value);
                dgResults.Columns[0].Header = "Par";
                var columnPair = dgResults.Columns[0] as DataGridTextColumn;
                var bindingPair = new Binding("Key.Name");
                columnPair.Binding = bindingPair;
            } else
            {
                dgResults.ItemsSource = quiz.LeaderboardPairTeam.OrderByDescending(item => item.Value);
                dgResults.Columns[0].Header = "Tim";
                var columnTeam = dgResults.Columns[0] as DataGridTextColumn;
                var bindingTeam = new Binding("Key.Name");
                columnTeam.Binding = bindingTeam;
            }
            dgResults.Columns[1].Header = "Bodovi";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
