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
    /// Interaction logic for MyTeams.xaml
    /// </summary>
    public partial class MyTeams : Window
    {
        TeamRepository teamRepository = new TeamRepository();
        public MyTeams()
        {
            InitializeComponent();
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshDataGrid();
        }

        private async Task RefreshDataGrid()
        {
            List<Team> teams = await teamRepository.ShowMyTeams(UserSessionService.Instance.LoggedInUser);

            dgTeams.ItemsSource = teams;

            var columnCaptain = dgTeams.Columns[1] as DataGridTextColumn;
            var bindingCaptain = new Binding("Captain.Username");
            columnCaptain.Binding = bindingCaptain;

            var columnMembers = dgTeams.Columns[2] as DataGridTextColumn;
            var bindingMembers = new Binding("ConcatenatedUsernames");
            columnMembers.Binding = bindingMembers;

            dgTeams.Columns[0].Header = "Naziv";
            dgTeams.Columns[1].Header = "Kapetan";
            dgTeams.Columns[2].Header = "Članovi";
            dgTeams.Columns[3].Header = "Vrsta";
            dgTeams.Columns[4].Header = "Popunjenost";
            dgTeams.Columns[5].Header = "Vidljivost";
            dgTeams.Columns[6].Visibility = Visibility.Hidden;
            dgTeams.Columns[7].Visibility = Visibility.Hidden;
            dgTeams.Columns[8].Visibility = Visibility.Hidden;
        }

        private async void btnLeave_Click(object sender, RoutedEventArgs e)
        {
            if (dgTeams.SelectedItem == null)
            {
                MessageBox.Show("Odaberite tim koji želite napustiti");
            } else if (dgTeams.SelectedItem is Team selectedTeam)
            {
                if (selectedTeam.Captain.Username == UserSessionService.Instance.LoggedInUser.Username)
                {
                    MessageBox.Show("Ne možete napustiti tim čiji ste kapetan");
                } else
                {
                    selectedTeam.Members.RemoveAt(selectedTeam.Members.FindIndex(user => user.Username == UserSessionService.Instance.LoggedInUser.Username));
                    if(selectedTeam.Occupancy == TeamOccupancy.Popunjen)
                    {
                        selectedTeam.Occupancy = TeamOccupancy.Slobodan;
                    }
                    await teamRepository.CreateOrUpdateTeam(selectedTeam);
                    MessageBox.Show("Napustili ste tim");
                }
            }
            (sender as Button).Focusable = false;
            this.Focus();
            await RefreshDataGrid();
        }

        private async void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dgTeams.SelectedItem == null)
            {
                MessageBox.Show("Odaberite tim koji želite ukloniti");
            } else if (dgTeams.SelectedItem is Team selectedTeam)
            {
                if (selectedTeam.Captain.Username != UserSessionService.Instance.LoggedInUser.Username)
                {
                    MessageBox.Show("Ne možete ukloniti tim ako niste kapetan");
                } else
                {
                    await teamRepository.DeleteTeam(selectedTeam.Name);
                    MessageBox.Show("Tim uspješno uklonjen");
                }
            }
            (sender as Button).Focusable = false;
            this.Focus();
            await RefreshDataGrid();
        }

        private async void btnChangeVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (dgTeams.SelectedItem == null)
            {
                MessageBox.Show("Odaberite tim kojem želite promijeniti vidljivost");
            } else if (dgTeams.SelectedItem is Team selectedTeam)
            {
                if (selectedTeam.Captain.Username != UserSessionService.Instance.LoggedInUser.Username)
                {
                    MessageBox.Show("Ne možete promijeniti vidljivost tima ako niste kapetan");
                } else
                {
                    if (selectedTeam.Visibility == TeamVisibility.Javan)
                    {
                        selectedTeam.Visibility = TeamVisibility.Privatan;
                        selectedTeam.AccessCode = GenerateAccessCode();
                        MessageBox.Show($"Vidljivost tima uspješno promijenjena.\n\nPristupni kod: {selectedTeam.AccessCode}");
                    } else
                    {
                        selectedTeam.Visibility = TeamVisibility.Javan;
                        selectedTeam.AccessCode = "";
                        MessageBox.Show("Vidljivost tima uspješno promijenjena");
                    }
                    await teamRepository.CreateOrUpdateTeam(selectedTeam);
                }
            }
            (sender as Button).Focusable = false;
            this.Focus();
            await RefreshDataGrid();
        }

        private string GenerateAccessCode()
        {
            Random random = new Random();
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TeamManagement teamManagement = new TeamManagement();
            teamManagement.Show();
            this.Close();
        }
    }
}
