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
    /// Interaction logic for Leaderboards.xaml
    /// </summary>
    public partial class Leaderboards : Window
    {
        UserRepository userRepository = new UserRepository();
        public Leaderboards()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<User> allUsers = await userRepository.GetAllUsers();

            List<User> sortSoloPoints = allUsers.OrderByDescending(user => user.SoloPoints).ToList();
            List<User> sortPairPoints = allUsers.OrderByDescending(user => user.PairPoints).ToList();
            List<User> sortTeamPoints = allUsers.OrderByDescending(user => user.TeamPoints).ToList();

            dgSoloPoints.ItemsSource = sortSoloPoints;
            dgPairPoints.ItemsSource = sortPairPoints;
            dgTeamPoints.ItemsSource = sortTeamPoints;

            dgSoloPoints.Columns[0].Header = "Korisničko ime";
            dgSoloPoints.Columns[1].Visibility = Visibility.Hidden;
            dgSoloPoints.Columns[2].Visibility = Visibility.Hidden;
            dgSoloPoints.Columns[3].Visibility = Visibility.Hidden;
            dgSoloPoints.Columns[4].Visibility = Visibility.Hidden;
            dgSoloPoints.Columns[5].Header = "Bodovi solo";
            dgSoloPoints.Columns[6].Visibility = Visibility.Hidden;
            dgSoloPoints.Columns[7].Visibility = Visibility.Hidden;
            dgSoloPoints.Columns[8].Visibility = Visibility.Hidden;
            dgSoloPoints.Columns[9].Visibility = Visibility.Hidden;

            dgPairPoints.Columns[0].Header = "Korisničko ime";
            dgPairPoints.Columns[1].Visibility = Visibility.Hidden;
            dgPairPoints.Columns[2].Visibility = Visibility.Hidden;
            dgPairPoints.Columns[3].Visibility = Visibility.Hidden;
            dgPairPoints.Columns[4].Visibility = Visibility.Hidden;
            dgPairPoints.Columns[5].Visibility = Visibility.Hidden;
            dgPairPoints.Columns[6].Header = "Bodovi parovi";
            dgPairPoints.Columns[7].Visibility = Visibility.Hidden;
            dgPairPoints.Columns[8].Visibility = Visibility.Hidden;
            dgPairPoints.Columns[9].Visibility = Visibility.Hidden;

            dgTeamPoints.Columns[0].Header = "Korisničko ime";
            dgTeamPoints.Columns[1].Visibility = Visibility.Hidden;
            dgTeamPoints.Columns[2].Visibility = Visibility.Hidden;
            dgTeamPoints.Columns[3].Visibility = Visibility.Hidden;
            dgTeamPoints.Columns[4].Visibility = Visibility.Hidden;
            dgTeamPoints.Columns[5].Visibility = Visibility.Hidden;
            dgTeamPoints.Columns[6].Visibility = Visibility.Hidden;
            dgTeamPoints.Columns[7].Header = "Bodovi timski";
            dgTeamPoints.Columns[8].Visibility = Visibility.Hidden;
            dgTeamPoints.Columns[9].Visibility = Visibility.Hidden;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
