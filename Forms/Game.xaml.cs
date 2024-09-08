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
using System.Windows.Threading;

namespace Kvizazov.Forms
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        private Quiz quiz;
        private User user;
        private Team team;
        private float totalScore = 0;
        private DispatcherTimer timer;
        private int timeLeft;
        private List<Question> allQuestions;
        private Question question;
        private Random random = new Random();
        private int randomIndex;
        private List<string> answers;
        private string correctAnswer;
        private int questionsLeft;
        private Color blue = (Color)ColorConverter.ConvertFromString("#2196F3");
        private Color green = (Color)ColorConverter.ConvertFromString("#4CAF50");
        private Color red = (Color)ColorConverter.ConvertFromString("#F44336");

        QuizRepository quizRepository = new QuizRepository();
        UserRepository userRepository = new UserRepository();
        TeamRepository teamRepository = new TeamRepository();

        public Game(Quiz _quiz, User _user, Team _team)
        {
            InitializeComponent();
            quiz = _quiz;
            user = _user;
            team = _team;
            timeLeft = _quiz.SecondsPerQuestion;
            allQuestions = _quiz.Questions;
            questionsLeft = _quiz.NumQuestions;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (quiz.Type == QuizType.Individualni)
            {
                lblTitle.Content = $"Solo - {user.Username}";
            } else if (quiz.Type == QuizType.Parovi)
            {
                lblTitle.Content = $"Par - {team.Name}";
            } else
            {
                lblTitle.Content = $"Tim - {team.Name}";
            }

            progressBar.Maximum = quiz.NumQuestions;

            questionsLeft--;
            NextQuestion();
        }

        private void NextQuestion()
        {
            Dispatcher.Invoke(() =>
            {
                btnAnswer1.IsEnabled = true;
                btnAnswer2.IsEnabled = true;
                btnAnswer3.IsEnabled = true;
                btnAnswer4.IsEnabled = true;

                progressBar.Value++;

                lblTimer.Content = quiz.SecondsPerQuestion;

                randomIndex = random.Next(allQuestions.Count);
                question = allQuestions[randomIndex];
                allQuestions.RemoveAt(randomIndex);

                lblQuestion.Content = question.QuestionText;

                correctAnswer = question.CorrectAnswer;

                answers = new List<string>();

                answers.Add(correctAnswer);
                answers.Add(question.WrongAnswers[0]);
                answers.Add(question.WrongAnswers[1]);
                answers.Add(question.WrongAnswers[2]);

                answers = answers.OrderBy(a => random.Next()).ToList();

                btnAnswer1.Content = answers[0];
                btnAnswer2.Content = answers[1];
                btnAnswer3.Content = answers[2];
                btnAnswer4.Content = answers[3];

                btnAnswer1.Background = new SolidColorBrush(blue);
                btnAnswer2.Background = new SolidColorBrush(blue);
                btnAnswer3.Background = new SolidColorBrush(blue);
                btnAnswer4.Background = new SolidColorBrush(blue);

                timeLeft = quiz.SecondsPerQuestion;
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                lblTimer.Content = timeLeft;
            } else
            {
                timer.Stop();

                btnAnswer1.IsEnabled = false;
                btnAnswer2.IsEnabled = false;
                btnAnswer3.IsEnabled = false;
                btnAnswer4.IsEnabled = false;

                lblTimer.Content = "X";
                btnAnswer1.Background = new SolidColorBrush(red);
                btnAnswer2.Background = new SolidColorBrush(red);
                btnAnswer3.Background = new SolidColorBrush(red);
                btnAnswer4.Background = new SolidColorBrush(red);

                if (questionsLeft > 0)
                {
                    questionsLeft--;
                    Task.Delay(1500).ContinueWith(t => NextQuestion());

                } else
                {
                    Task.Delay(1500).ContinueWith(t => EndGame());
                }

            }
        }

        private void btnAnswer_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            btnAnswer1.IsEnabled = false;
            btnAnswer2.IsEnabled = false;
            btnAnswer3.IsEnabled = false;
            btnAnswer4.IsEnabled = false;

            Button buttonClicked = sender as Button;

            buttonClicked.Focusable = false;
            this.Focus();

            if (buttonClicked.Content.ToString() == correctAnswer)
            {
                buttonClicked.Background = new SolidColorBrush(green);

                float timeScore = (float)timeLeft / quiz.SecondsPerQuestion;
                timeScore = timeScore / quiz.NumQuestions;
                totalScore += (1+timeScore);
            } else
            {
                buttonClicked.Background = new SolidColorBrush(red);

                if(btnAnswer1.Content.ToString() == correctAnswer)
                {
                    btnAnswer1.Background = new SolidColorBrush(green);
                } else if (btnAnswer2.Content.ToString() == correctAnswer)
                {
                    btnAnswer2.Background = new SolidColorBrush(green);
                } else if (btnAnswer3.Content.ToString() == correctAnswer)
                {
                    btnAnswer3.Background = new SolidColorBrush(green);
                } else
                {
                    btnAnswer4.Background = new SolidColorBrush(green);
                }
            }

            if (questionsLeft > 0)
            {
                questionsLeft--;
                Task.Delay(1500).ContinueWith(t => NextQuestion());
            } else
            {
                Task.Delay(1500).ContinueWith(t => EndGame());
            }
        }

        private async Task EndGame()
        {
            await Dispatcher.Invoke(async () =>
            {
                Quiz quizData = await quizRepository.GetQuizById(this.quiz.Id);
                if (totalScore > 0)
                {
                    totalScore = totalScore / (quiz.NumQuestions + 1) * 100;
                    if (quizData.Type == QuizType.Individualni)
                    {
                        User userData = await userRepository.GetUserByUsername(this.user.Username);
                        quizData.LeaderboardSolo.Add(new KeyValuePair<User, float>(user, totalScore));
                        quizData.LeaderboardSolo.RemoveAll(entry => entry.Key.Username == user.Username && entry.Value == 0);
                        userData.SoloPoints += totalScore;
                        await userRepository.CreateOrUpdateUser(userData);
                    } else
                    {
                        Team teamData = await teamRepository.GetTeamByName(this.team.Name);
                        quizData.LeaderboardPairTeam.Add(new KeyValuePair<Team, float>(team, totalScore));
                        quizData.LeaderboardPairTeam.RemoveAll(entry => entry.Key.Name == team.Name && entry.Value == 0);
                        foreach (User member in teamData.Members)
                        {
                            if (quizData.Type == QuizType.Parovi)
                            {
                                member.PairPoints += totalScore;
                            } else
                            {
                                member.TeamPoints += totalScore;
                            }
                            await userRepository.CreateOrUpdateUser(member);
                        }
                    }
                    await quizRepository.CreateOrUpdateQuiz(quizData);
                }

                CurrentQuizLeaderboard currentQuizLeaderboard = new CurrentQuizLeaderboard(quizData);
                currentQuizLeaderboard.Show();
                this.Close();

            });
        }
    }
}
