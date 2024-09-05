using Kvizazov.Model;
using Kvizazov.Repositories;
using Kvizazov.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kvizazov.Forms
{

    public partial class NewTeam : Window
    {
        TeamRepository teamRepository = new TeamRepository();

        public NewTeam()
        {
            InitializeComponent();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            bool emptyFields = txtName.Text == "" || cmbType.SelectedItem == null || cmbVisibility.SelectedItem == null;
            if (emptyFields)
            {
                MessageBox.Show("Sva polja su obavezna. Ponovite unos novog tima.");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            }

            Team team = new Team
            {
                Name = txtName.Text,
                Captain = UserSessionService.Instance.LoggedInUser,
                Members = new List<User>(),
                Type = (TeamType)cmbType.SelectedItem,
                Occupancy = TeamOccupancy.Slobodan,
                Visibility = (TeamVisibility)cmbVisibility.SelectedItem,
                AccessCode = (TeamVisibility)cmbVisibility.SelectedItem == TeamVisibility.Privatan ? txtAccessCode.Text : ""
            };
            team.Members.Add(UserSessionService.Instance.LoggedInUser);

            try
            {
                bool teamExists = await teamRepository.CheckIfTeamExists(team.Name);
                if (teamExists)
                {
                    MessageBox.Show("Već postoji tim s navedenim imenom. Ponovite kreiranje tima.");
                    (sender as Button).Focusable = false;
                    this.Focus();
                    return;
                } else
                {
                    await teamRepository.CreateOrUpdateTeam(team);
                    MessageBox.Show("Tim uspješno kreiran");
                    (sender as Button).Focusable = false;
                    this.Focus();
                }
                
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbType.ItemsSource = Enum.GetValues(typeof(TeamType));
            cmbVisibility.ItemsSource = Enum.GetValues(typeof(TeamVisibility));
        }

        private void cmbVisibility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TeamVisibility selectedVisibility = (TeamVisibility)cmbVisibility.SelectedItem;
            if (selectedVisibility == TeamVisibility.Privatan)
            {
                txtAccessCode.Text = GenerateAccessCode();
            } else
            {
                txtAccessCode.Text = "";
            }
        }

        private string GenerateAccessCode()
        {
            Random random = new Random();
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TeamManagement teamManagement = new TeamManagement();
            teamManagement.Show();
            this.Close();
        }
    }
}
