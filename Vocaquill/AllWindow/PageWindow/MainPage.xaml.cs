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
using System.Windows.Threading;

namespace Vocaquill.AllWindow.PageWindow
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        #region Timer
        private DispatcherTimer timer;
        private TimeSpan timeElapsed;
        private bool isRecording = false;
        #endregion
        private SettingWindowPage settWinPage;
        public MainPage()
        {
            InitializeComponent();
            settWinPage = new SettingWindowPage();
            #region Timer initialize
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            #endregion
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1));
            this.TimerRecordTB.Text = timeElapsed.ToString(@"mm\:ss");
        }
        private void SettingsBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Натиснуто на Border!");
            (App.Current.MainWindow as MainWindow).pageFrame.Navigate(settWinPage);
        }

        private void StartStopRecordBT_Click(object sender, RoutedEventArgs e)
        {
            if (!isRecording)
            {
                TimerRecordTB.Text = "00:00";
                timeElapsed = TimeSpan.Zero;
                timer.Start();
                StartStopRecordBT.Content = "Зупинити запис";
                isRecording = true;
            }
            else
            {
                timer.Stop();
                StartStopRecordBT.Content = "Старт запису";
                isRecording = false;
            }
        }

        private void LogoutBT_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.modalFrame.Navigate(new LoginWindowPage());
        }
    }
}
