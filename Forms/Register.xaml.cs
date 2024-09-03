﻿using Kvizazov.Model;
using Kvizazov.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Register.xaml
    /// </summary>
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
            //check if there are no empty textboxes
            bool emptyFields = user.Username == "" || user.Password == "" || user.Email == "" || user.Name == "" || user.Surname == "";
            if(!isValidEmailFormat || emptyFields)
            {
                MessageBox.Show("Sva polja su obavezna. Email mora biti u ispravnom formatu. Ponovite registraciju.");
                Login login = new Login();
                login.Show();
                this.Close();
                return;
            }
            try
            {
                bool uniqueUsername = await userRepository.CheckForExistingUserParameter("\"username\"", $"\"{user.Username}\"");
                bool uniqueEmail = await userRepository.CheckForExistingUserParameter("\"username\"", $"\"{user.Email}\"");
                if (!uniqueUsername)
                {
                    MessageBox.Show("Korisničko ime već postoji. Ponovite registraciju.");
                } else if (!uniqueEmail)
                {
                    MessageBox.Show("Ovaj email je već registriran. Ponovite registraciju.");
                } else
                {
                    await userRepository.RegisterUser(user);
                    MessageBox.Show("Registracija uspješna. Možete se prijaviti.");
                }                
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Login login1 = new Login();
            login1.Show();
            this.Close();
        }
    }
}
