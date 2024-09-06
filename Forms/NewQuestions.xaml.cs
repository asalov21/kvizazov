using Kvizazov.Model;
using Kvizazov.Repositories;
using Kvizazov.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for NewQuestions.xaml
    /// </summary>
    public partial class NewQuestions : Window
    {
        QuestionRepository questionRepository = new QuestionRepository();
        public NewQuestions()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!UserSessionService.Instance.IsLoggedInUserAdmin())
            {
                btnApprove.IsEnabled = false;
                btnApprove.Background = Brushes.Gray;
            }
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

        private async void btnSuggest_Click(object sender, RoutedEventArgs e)
        {
            bool emptyFields = txtQuestion.Text == "" || txtCorrectAnswer.Text == "" || txtWrongAnswer1.Text == "" || txtWrongAnswer2.Text == "" || txtWrongAnswer3.Text == "";
            if (emptyFields)
            {
                MessageBox.Show("Sva polja su obavezna. Ponovite unos novog pitanja.");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            }

            int nextPotentialQuestionId = await questionRepository.GetNextPotentialQuestionId();
            int nextRealQuestionId = await questionRepository.GetNextQuestionId();

            int nextQuestionId = nextPotentialQuestionId > nextRealQuestionId ? nextPotentialQuestionId : nextRealQuestionId;

            Question question = new Question
            {
                Id = nextQuestionId,
                QuestionText = txtQuestion.Text,
                CorrectAnswer = txtCorrectAnswer.Text,
                WrongAnswers = new List<string>()
            };
            question.WrongAnswers.Add(txtWrongAnswer1.Text);
            question.WrongAnswers.Add(txtWrongAnswer2.Text);
            question.WrongAnswers.Add(txtWrongAnswer3.Text);

            await questionRepository.CreatePotentialQuestion(question);
            MessageBox.Show("Pitanje uspješno predloženo. Čeka odobrenje administratora.");
            (sender as Button).Focusable = false;
            this.Focus();
            txtQuestion.Text = "";
            txtCorrectAnswer.Text = "";
            txtWrongAnswer1.Text = "";
            txtWrongAnswer2.Text = "";
            txtWrongAnswer3.Text = "";
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            ApproveQuestions approveQuestions = new ApproveQuestions();
            approveQuestions.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
