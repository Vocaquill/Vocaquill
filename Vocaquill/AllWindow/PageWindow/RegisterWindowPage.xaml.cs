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
    /// Interaction logic for RegisterWindowPage.xaml
    /// </summary>
    public partial class RegisterWindowPage : Page
    {
        public RegisterWindowPage(UserViewModel userView)
        {
            InitializeComponent();
            this.DataContext = userView;
        }
    }
}
