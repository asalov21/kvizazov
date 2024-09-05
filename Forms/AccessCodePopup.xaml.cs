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
    /// Interaction logic for AccessCodePopup.xaml
    /// </summary>
    public partial class AccessCodePopup : Window
    {
        public string AccessCodeUserInput { get; private set; }
        public string TeamAccessCode { get; private set; }
        public AccessCodePopup(string _teamAccessCode)
        {
            InitializeComponent();
            TeamAccessCode = _teamAccessCode;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            AccessCodeUserInput = txtAccessCode.Text;
            DialogResult = AccessCodeUserInput == TeamAccessCode;
        }
    }
}
