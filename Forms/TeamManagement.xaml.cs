using Kvizazov.Model;
using Kvizazov.Repositories;
using Kvizazov.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Kvizazov.Forms
{

    public partial class TeamManagement : Window
    {
        TeamRepository teamRepository = new TeamRepository();

        public TeamManagement()
        {
            InitializeComponent();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            bool emptyFilters = cmbType.SelectedItem == null || cmbOccupancy.SelectedItem == null || cmbVisibility.SelectedItem == null;
            if (emptyFilters)
            {
                MessageBox.Show("Trebate odabrati vrstu, popunjenost i vidljivost.");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            }

            List<Team> teams = await teamRepository.ShowFilteredTeams((TeamType)cmbType.SelectedItem, (TeamOccupancy)cmbOccupancy.SelectedItem, (TeamVisibility)cmbVisibility.SelectedItem);

            dgTeams.ItemsSource = teams;

            (sender as Button).Focusable = false;
            this.Focus();

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

        private async void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            if(dgTeams.SelectedItem == null)
            {
                MessageBox.Show("Odaberite tim kojem se želite pridružiti");
            }else if(dgTeams.SelectedItem is Team selectedTeam)
            {
                if (selectedTeam.Members.Select(user => user.Username).ToList().Contains(UserSessionService.Instance.LoggedInUser.Username))
                {
                    MessageBox.Show("Već ste član ovog tima");
                } else if(selectedTeam.Occupancy == TeamOccupancy.Popunjen)
                {
                    MessageBox.Show("Tim je pun");
                } else if(selectedTeam.Visibility == TeamVisibility.Privatan)
                {
                    AccessCodePopup accessCodePopup = new AccessCodePopup(selectedTeam.AccessCode);
                    if(accessCodePopup.ShowDialog() == false)
                    {
                        MessageBox.Show("Pogrešan pristupni kod.");
                    } else
                    {
                        selectedTeam.Members.Add(UserSessionService.Instance.LoggedInUser);
                        if ((selectedTeam.Type == TeamType.Par && selectedTeam.Members.Count == 2) || (selectedTeam.Type == TeamType.Tim && selectedTeam.Members.Count == 4))
                        {
                            selectedTeam.Occupancy = TeamOccupancy.Popunjen;
                        }
                        await teamRepository.CreateOrUpdateTeam(selectedTeam);
                        MessageBox.Show("Uspješno ste se pridružili timu");
                    }
                } else
                {
                    selectedTeam.Members.Add(UserSessionService.Instance.LoggedInUser);
                    if((selectedTeam.Type == TeamType.Par && selectedTeam.Members.Count == 2) || (selectedTeam.Type == TeamType.Tim && selectedTeam.Members.Count == 4))
                    {
                        selectedTeam.Occupancy = TeamOccupancy.Popunjen;
                    }
                    await teamRepository.CreateOrUpdateTeam(selectedTeam);
                    MessageBox.Show("Uspješno ste se pridružili timu");
                }
            }
            (sender as Button).Focusable = false;
            this.Focus();
            dgTeams.ItemsSource = null;
        }

        private void btnNewTeam_Click(object sender, RoutedEventArgs e)
        {
            NewTeam newTeam = new NewTeam();
            newTeam.Show();
            this.Close();
        }

        private void btnMyTeams_Click(object sender, RoutedEventArgs e)
        {
            MyTeams myTeams = new MyTeams();
            myTeams.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbType.ItemsSource = Enum.GetValues(typeof(TeamType));
            cmbOccupancy.ItemsSource = Enum.GetValues(typeof(TeamOccupancy));
            cmbVisibility.ItemsSource = Enum.GetValues(typeof(TeamVisibility));
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
