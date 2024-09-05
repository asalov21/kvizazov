using System.Windows;
using System.Windows.Input;

namespace Kvizazov.Forms
{
    public class BaseForm : Window
    {
        public BaseForm()
        {
            this.KeyDown += BaseForm_KeyDown;
        }

        private void BaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
