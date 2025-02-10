using Microsoft.Win32;
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

namespace Vocaquill.AllWindow.PageWindow
{
    /// <summary>
    /// Interaction logic for SettingWindowPage.xaml
    /// </summary>
    public partial class SettingWindowPage : Page
    {
        private Page _prevPage;
        public SettingWindowPage()
        {
            InitializeComponent();
        }
        public SettingWindowPage(Page prevPage)
        {
            InitializeComponent();
            _prevPage = prevPage;
        }
        private void ChoisePathBT_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                this.DownloadPathTB.Text = openFileDialog.FileName;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Border)
                return;
            (App.Current.MainWindow as MainWindow).pageFrame.Navigate(_prevPage);
        }
    }
}
