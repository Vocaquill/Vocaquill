//Vlad and Kostya

using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using DAL.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Vocaquill.AllWindow.PageWindow;
using Vocaquill.Commands;
using Vocaquill.Singleton;

namespace Vocaquill.AllWindow.ViewModels
{
    public class UserViewModel: INotifyPropertyChanged
    {
        // Login : admin123
        // Password : StrongPass123
        public  BaseCommand RegisterInProggram
        {
            get
            {
                return _registerInProggram ??= new BaseCommand(async _ =>
                {
                    try
                    {
                        await _dBSingleton.DBService.UserService.GetUserByLoginAsync(User_register.Login);
                        MessageBox.Show("Такий користувач існує!");
                    }
                    catch
                    {
                        try
                        {
                            var newUser = new UserDTO()
                            {
                                Login = User_register.Login,
                                Email = User_register.Email,
                                Password = User_register.Password,
                                Name = User_register.Login
                            };

                            await _dBSingleton.DBService.UserService.AddUserAsync(newUser);

                            DBSingleton.Instance.CurrentUser = newUser;

                            var mainWindow = (MainWindow)Application.Current.MainWindow;
                            mainWindow.modalFrame.Navigate(new MainPage(new RecordViewModel()));
                        }
                        catch(Exception ex) 
                        {
                            MessageBox.Show(ex.ToString());
                        }

                    }
                });
            }
        }
        public BaseCommand LoginInProggram
        {
            get
            {
                return _loginInProggram ??= new BaseCommand(async _ =>
                {
                    try
                    {
                        DBSingleton.Instance.CurrentUser = await _dBSingleton.DBService.UserService.GetUserByLoginAndPasswordAsync(User_login.Login, User_login.Password);
                        var mainWindow = (MainWindow)Application.Current.MainWindow;
                        mainWindow.modalFrame.Navigate(new MainPage(new RecordViewModel()));
                    }
                    catch(Exception ex)
                    {
                        this.ErrorMessage = "Неправильний логін або пароль";
                    }
                });
            }
        }
        public BaseCommand NavigateCommand
        {
            get
            {
                return _navigateCommand ??= new BaseCommand(async _ =>
                {
                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.modalFrame.Navigate(new RegisterWindowPage(new UserViewModel(_dBSingleton)));
                });
            }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));  
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public UserViewModel(DBSingleton singleton)
        {
            User_login = new UserDTO();
            User_register = new UserDTO();
            _dBSingleton = singleton;
        }
        #region Validation

        #endregion

        private DBSingleton _dBSingleton;
        public UserDTO User_login { get; set; } 
        public UserDTO User_register { get; set; }
        private BaseCommand _registerInProggram;
        private BaseCommand _loginInProggram;
        private BaseCommand _navigateCommand;
    }
}
