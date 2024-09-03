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
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        UserRepository userRepository = new UserRepository();
        public Profile()
        {
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;
            txtEmail.IsEnabled = true;
            txtName.IsEnabled = true;
            txtSurname.IsEnabled = true;
            txtPassword.IsEnabled = true;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Hidden;
            txtEmail.IsEnabled = false;
            txtName.IsEnabled = false;
            txtSurname.IsEnabled = false;
            txtPassword.IsEnabled = false;
            User user = new User()
            {
                Username = UserSessionService.Instance.LoggedInUser.Username,
                Email = txtEmail.Text,
                Name = txtName.Text,
                Surname = txtSurname.Text,
                Password = txtPassword.Password
            };
            await userRepository.EditUser(user);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            UserSessionService.Instance.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnSave.Visibility = Visibility.Hidden;
            User user = await userRepository.GetUserByUsername(UserSessionService.Instance.LoggedInUser.Username);
            lblRoleUsername.Content = user.Role + " - " + user.Username;
            txtEmail.Text = user.Email;
            txtEmail.IsEnabled = false;
            txtName.Text = user.Name;
            txtName.IsEnabled = false;
            txtSurname.Text = user.Surname;
            txtSurname.IsEnabled = false;
            txtPassword.Password = user.Password;
            txtPassword.IsEnabled = false;
            lblSoloPoints.Content = $"Bodovi solo: {user.SoloPoints}";
            lblPairPoints.Content = $"Bodovi parovi: {user.PairPoints}";
            lblTeamPoints.Content = $"Bodovi timski: {user.TeamPoints}";
        }
    }
}
