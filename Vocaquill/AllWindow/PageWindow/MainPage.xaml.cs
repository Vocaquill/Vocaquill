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
using Vocaquill.AllWindow.ViewModels;

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
        public MainPage(RecordViewModel recordVM)
        {
            InitializeComponent();
            InitializeTimer();

            settWinPage = new SettingWindowPage();

            recordVM.FunctionalityPage = this;
            this.DataContext = recordVM;
        }

        public void ChangeTimerState()
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
                StartStopRecordBT.Content = "Новий запис";
                isRecording = false;
            }
        }
        public void ShowInfo(string text) 
        {
            this.LectureTextBox.Text = text;
        }
        public void ChangeFunctionality(bool state) 
        {
            if (state)
            {
                this.loadingGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                this.loadingGrid.Visibility = Visibility.Visible;
            }
        }

        public void ShowRequestPopup(bool state) 
        {
            this.requestPopup.IsOpen = state;
        }

        private void InitializeTimer() 
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1));
            this.TimerRecordTB.Text = timeElapsed.ToString(@"mm\:ss");
        }
        private void SettingsBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).pageFrame.Navigate(settWinPage);
        }

        private void LogoutBT_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            //mainWindow.modalFrame.Navigate(new LoginWindowPage());            --------------------------------
        }

        private void autoSettingsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.topicTB.Text = "Визначити автоматично";
            this.sumSizeTB.Text = "Великий (1-2 листки A4)";
            this.sumSizeTB.Text = "Українська";

            this.langTB.IsEnabled = false;
            this.sumSizeTB.IsEnabled = false;
            this.topicTB.IsEnabled = false;
        }

        private void autoSettingsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.langTB.IsEnabled = true;
            this.sumSizeTB.IsEnabled = true;
            this.topicTB.IsEnabled = true;
        }
    }
}
