//Vlad and Kostya

using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using DAL.Repositories;
using System.Windows;
using System.Windows.Controls.Primitives;
using Vocaquill.AllWindow.PageWindow;
using Vocaquill.Commands;

namespace Vocaquill.AllWindow.ViewModels
{
    public class UserViewModel
    {
        private void CloseModal()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.modalFrame.Navigate(new MainPage());
        }
        public BaseCommand LoginInProggram
        {
            get
            {
                return _testCommand ??= new BaseCommand(async _ =>
                {
                    try
                    {
                        
                    }
                    catch
                    {

                    }
                });
            }
        }
        public UserViewModel()
        {
            User = new UserDTO();
        }
        public UserDTO User { get; set; }
        private IUserService _userService;

        private BaseCommand _testCommand;
    }
}
