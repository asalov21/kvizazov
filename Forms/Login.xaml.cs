using Kvizazov.Model;
using Kvizazov.Repositories;
using Kvizazov.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Kvizazov.Forms
{

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
            string password = txtPassword.Password;

            bool emptyFields = username == "" || password == "";
            if (emptyFields)
            {
                MessageBox.Show("Sva polja su obavezna. Ponovite prijavu.");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            }
            try
            {   
                bool userRegistered = await userRepository.CheckIfUserExists(username);
                if(!userRegistered)
                {
                    MessageBox.Show("Korisničko ime ne postoji. Registrirajte se.");
                    (sender as Button).Focusable = false;
                    this.Focus();
                    return;
                } else
                {
                    User userToLogin = await userRepository.GetUserByUsername(username);
                    bool loginCheck = userToLogin.Password == password;
                    if (loginCheck)
                    {
                        MessageBox.Show("Prijava uspješna.");
                        User user = await userRepository.GetUserByUsername(username);
                        UserSessionService.Instance.Login(user);
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    } else
                    {
                        MessageBox.Show("Korisničko ime i lozinka se ne podudaraju. Ponovite prijavu.");
                        (sender as Button).Focusable = false;
                        this.Focus();
                        return;
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Close();
        }
    }
}
