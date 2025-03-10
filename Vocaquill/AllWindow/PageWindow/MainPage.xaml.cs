﻿using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
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

        public void ShowPromptSettings(bool state) 
        {
            this.promptGrid.Visibility = (state)? Visibility.Visible : Visibility.Hidden;
        }
        public void ShowQueriesList(bool state)
        {
            this.queryHistoryGrid.Visibility = (state) ? Visibility.Visible : Visibility.Hidden;
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
            mainWindow.modalFrame.Navigate(new LoginWindowPage(new UserViewModel()));
        }

        private void autoSettingsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var question = (this.DataContext as RecordViewModel).QuestionSettings;

            question.LectureTopic = "Determine automatically";
            question.SummarySize = "Determine automatically";
            question.Language = "Ukrainian";

            this.langTB.Text = question.Language;
            this.sumSizeTB.Text = question.SummarySize;
            this.topicTB.Text = question.LectureTopic;

            this.autoSettingsCheckBox.IsChecked = false;
        }

        private void exitBT_Click(object sender, RoutedEventArgs e)
        {
            ShowPromptSettings(false);
        }

        public void ChangeConvertBtState(bool state)
        {
            this.convertBT.IsEnabled = state;
        }

        private void exitHistoryBT_Click(object sender, RoutedEventArgs e)
        {
            ShowQueriesList(false);
        }

        private void queryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                if (listBox.SelectedItem is QueryDTO selectedQuery)
                {
                    this.downloadItemBT.IsEnabled = true;
                }
                else
                {
                    this.downloadItemBT.IsEnabled = false;
                }
            }
        }
    }
}
