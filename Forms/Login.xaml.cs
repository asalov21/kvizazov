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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kvizazov.Forms
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        UserRepository userRepository = new UserRepository();

        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            try
            {   
                bool userRegistered = await userRepository.CheckIfUserExists(username);
                if(!userRegistered)
                {
                    MessageBox.Show("Korisničko ime ne postoji. Registrirajte se.");
                } else
                {
                    bool loginCheck = await userRepository.LoginCheckUsernamePassword(username, password);
                    if (loginCheck)
                    {
                        MessageBox.Show("Prijava uspješna.");
                    } else
                    {
                        MessageBox.Show("Korisničko ime i lozinka se ne podudaraju. Ponovite prijavu.");
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            //replace this window with Register.xaml window
            Register register = new Register();
            register.Show();
            this.Close();
        }
    }
}
