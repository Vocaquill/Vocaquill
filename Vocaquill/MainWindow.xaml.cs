using BLL.Models;
using System.ComponentModel;
using System.Text;
using System.Windows;
using Vocaquill.AllWindow.PageWindow;
using Vocaquill.Singleton;
using Vocaquill.AllWindow.ViewModels;
using System.Windows.Controls;

namespace Vocaquill
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            modalFrame.Content = new LoginWindowPage(new UserViewModel(DBSingleton.Instance));
        }

        public void ChangePage(Page page) 
        {
            modalFrame.Navigate(page);
        }
    }
}