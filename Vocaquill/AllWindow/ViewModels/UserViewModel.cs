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
        public BaseCommand LoginInProggram
        {
            get
            {
                return _loginCommand ??= new BaseCommand(async _ =>
                {
                    try
                    {
                        _mainWindow.ChangePage(new MainPage(new RecordViewModel()));
                    }
                    catch
                    {

                    }
                });
            }
        }
        public UserViewModel()
        {
            _mainWindow = (MainWindow)Application.Current.MainWindow;
        }
        private MainWindow _mainWindow;

        private BaseCommand _loginCommand;
    }
}
