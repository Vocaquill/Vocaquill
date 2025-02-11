using BLL.Models;
using System.ComponentModel;
using System.Text;
using System.Windows;
using Vocaquill.AllWindow.PageWindow;
using Vocaquill.Singleton;

namespace Vocaquill
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            modalFrame.Content = new LoginWindowPage();
        }
    }
}