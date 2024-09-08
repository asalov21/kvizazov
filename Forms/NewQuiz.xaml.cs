using Kvizazov.Model;
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
using System.Windows.Shapes;

namespace Kvizazov.Forms
{
    /// <summary>
    /// Interaction logic for NewQuiz.xaml
    /// </summary>
    public partial class NewQuiz : Window
    {
        QuizRepository quizRepository = new QuizRepository();
        UserRepository userRepository = new UserRepository();

        public NewQuiz()
        {
            InitializeComponent();
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

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            bool emptyFields = dateTimeStart.Value == null || dateTimeEnd.Value == null || txtNumQuestions.Text == "" || cmbType.SelectedItem == null || txtSecondsPerQuestion.Text == "";
            int maxNumberOfQuestions = await new QuestionRepository().GetAllNonNullQuestionsNumber();
            if (emptyFields)
            {
                MessageBox.Show("Sva polja su obavezna. Ponovite unos novog kviza");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            } else if(dateTimeEnd.Value < dateTimeStart.Value)
            {
                MessageBox.Show("Datum završetka kviza ne može biti prije datuma početka kviza");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            }else if(int.Parse(txtNumQuestions.Text) > maxNumberOfQuestions)
            {
                MessageBox.Show("Ne postoji dovoljan broj pitanja u bazi. Smanjite broj pitanja.");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            }

            int nextQuizId = await quizRepository.GetNextQuizId();

            Quiz quiz = new Quiz
            {
                Id = nextQuizId,
                Type = (QuizType)cmbType.SelectedItem,
                Start = (DateTime)dateTimeStart.Value,
                End = (DateTime)dateTimeEnd.Value,
                NumQuestions = int.Parse(txtNumQuestions.Text),
                SecondsPerQuestion = int.Parse(txtSecondsPerQuestion.Text),
                Status = (DateTime.Now >= (DateTime)dateTimeStart.Value && DateTime.Now <= (DateTime)dateTimeEnd.Value) ? QuizStatus.Otvoren : QuizStatus.Zatvoren,
                LeaderboardSolo = new List<KeyValuePair<User, float>>(),
                LeaderboardPairTeam = new List<KeyValuePair<Team, float>>(),
                Questions = new List<Question>()
            };
            quiz.Questions = await new QuestionRepository().GetRandomQuestions(quiz.NumQuestions);

            if(quiz.Type == QuizType.Individualni)
            {
                quiz.LeaderboardSolo.Add(new KeyValuePair<User, float>(new User {Username = "default"}, 0));
            } else
            {
                quiz.LeaderboardPairTeam.Add(new KeyValuePair<Team, float>(new Team { Name = "default" }, 0));
            }

            await quizRepository.CreateOrUpdateQuiz(quiz);
            MessageBox.Show("Kviz uspješno kreiran");
            (sender as Button).Focusable = false;
            this.Focus();
            cmbType.SelectedItem = null;
            dateTimeStart.Value = null;
            dateTimeEnd.Value = null;
            txtNumQuestions.Text = "";
            txtSecondsPerQuestion.Text = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            QuizSearch quizSearch = new QuizSearch();
            quizSearch.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbType.ItemsSource = Enum.GetValues(typeof(QuizType));
        }
    }
}
