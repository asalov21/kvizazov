using Kvizazov.Model;
using Kvizazov.Repositories;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

            List<Team> teams = await teamRepository.GetAllTeams();
            
            dgTeams.ItemsSource = teams;

            (sender as Button).Focusable = false;
            this.Focus();

            /*
            var columnCaptain = dgTeams.Columns[1] as DataGridTextColumn;
            var bindingCaptain = new Binding("Captain.Username");
            columnCaptain.Binding = bindingCaptain;
            */

            /*
            string test = string.Join(",", teams[0].Members.Select(user => user.Username));

            var columnMembers = dgTeams.Columns[2] as DataGridTextColumn;
            var bindingMembers = new Binding("string.Join(\",\",Members.Select(user => user.Username))");
            columnMembers.Binding = bindingMembers;
            */
        }

        private async void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnNewTeam_Click(object sender, RoutedEventArgs e)
        {
            NewTeam newTeam = new NewTeam();
            newTeam.Show();
            this.Close();
        }

        private void btnMyTeams_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
