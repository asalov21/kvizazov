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
    /// Interaction logic for ApproveQuestions.xaml
    /// </summary>
    public partial class ApproveQuestions : Window
    {
        QuestionRepository questionRepository = new QuestionRepository();
        public ApproveQuestions()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshDataGrid();
        }

        private async Task RefreshDataGrid()
        {
            List<Question> potentialQuestions = await questionRepository.GetAllPotentialQuestions();

            List<Question> questionsToShow = new List<Question>();

            foreach(Question potentialQuestion in potentialQuestions)
            {
                if(potentialQuestion != null)
                {
                    questionsToShow.Add(potentialQuestion);
                }
            }

            dgQuestions.ItemsSource = questionsToShow;

            dgQuestions.Columns[0].Visibility = Visibility.Hidden;
            dgQuestions.Columns[1].Header = "Pitanje";
            dgQuestions.Columns[2].Header = "Točan odgovor";
            dgQuestions.Columns[3].Visibility = Visibility.Hidden;
            dgQuestions.Columns[4].Header = "Netočni odgovori";
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

        private async void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if(dgQuestions.SelectedItems.Count == 0)
            {
                MessageBox.Show("Morate odabrati pitanja koja želite odobriti.");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            } else
            {
                var allSelectedQuestions = dgQuestions.SelectedItems;

                foreach(var q in allSelectedQuestions)
                {
                    if(q is Question selectedQuestion)
                    {
                        await questionRepository.CreateQuestion(selectedQuestion);
                        await questionRepository.DeletePotentialQuestion(selectedQuestion);
                    }
                }

                (sender as Button).Focusable = false;
                this.Focus();
                await RefreshDataGrid();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            NewQuestions newQuestions = new NewQuestions();
            newQuestions.Show();
            this.Close();
        }

        private async void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dgQuestions.SelectedItems.Count == 0)
            {
                MessageBox.Show("Morate odabrati pitanja koja želite ukloniti.");
                (sender as Button).Focusable = false;
                this.Focus();
                return;
            } else
            {
                var allSelectedQuestions = dgQuestions.SelectedItems;

                foreach (var q in allSelectedQuestions)
                {
                    if (q is Question selectedQuestion)
                    {
                        await questionRepository.DeletePotentialQuestion(selectedQuestion);
                    }
                }

                (sender as Button).Focusable = false;
                this.Focus();
                await RefreshDataGrid();
            }
        }
    }
}
