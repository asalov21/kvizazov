using Kvizazov.Model;
using Kvizazov.Repositories;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Kvizazov.Forms
{

    public partial class Register : Window
    {
        UserRepository userRepository = new UserRepository();

        public Register()
        {
            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            User user = new User
            {
                Username = txtUsername.Text,
                Password = txtPassword.Password,
                Email = txtEmail.Text,
                Name = txtName.Text,
                Surname = txtSurname.Text,
                SoloPoints = 0,
                PairPoints = 0,
                TeamPoints = 0,
                Role = Role.User
            };
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            bool isValidEmailFormat = Regex.IsMatch(user.Email, emailPattern);

            bool emptyFields = user.Username == "" || user.Password == "" || user.Email == "" || user.Name == "" || user.Surname == "";
            if(!isValidEmailFormat || emptyFields)
            {
                MessageBox.Show("Sva polja su obavezna. Email mora biti u ispravnom formatu. Ponovite registraciju.");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            }
            try
            {
                bool usernameExists = await userRepository.CheckIfUserExists(user.Username);
                bool uniqueEmail = await userRepository.CheckForExistingUserParameter("\"email\"", $"\"{user.Email}\"");
                if (usernameExists)
                {
                    MessageBox.Show("Korisničko ime već postoji. Ponovite registraciju.");
                    (sender as Button).Focusable = false;
                    this.Focus();
                    return;
                } else if (!uniqueEmail)
                {
                    MessageBox.Show("Ovaj email je već registriran. Ponovite registraciju.");
                    (sender as Button).Focusable = false;
                    this.Focus();
                    return;
                } else
                {
                    await userRepository.RegisterOrUpdateUser(user);
                    MessageBox.Show("Registracija uspješna. Možete se prijaviti.");
                    Login login = new Login();
                    login.Show();
                    this.Close();
                }                
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
