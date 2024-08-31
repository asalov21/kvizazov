using Kvizazov.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserRepository userRepository = new UserRepository();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //replace window login with current window
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void btnTeamManagement_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnQuizSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPlayQuiz_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLeaderboards_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNewQuestions_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
