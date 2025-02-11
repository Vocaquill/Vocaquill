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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vocaquill.AllWindow.ViewModels;

namespace Vocaquill.AllWindow.PageWindow
{
    /// <summary>
    /// Interaction logic for LoginWindowPage.xaml
    /// </summary>
    public partial class LoginWindowPage : Page
    {
        public LoginWindowPage()
        {
            InitializeComponent();
        }
        private void CloseModal()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.modalFrame.Navigate(new MainPage(new RecordViewModel()));
        }
        private void LoginBT_Click(object sender, RoutedEventArgs e)
        {
            CloseModal();

        }
    }
}
