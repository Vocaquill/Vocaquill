using System.Windows;

namespace Vocaquill.AllWindow.Additionals
{
    public static class DynamicDesigner
    {
        public static void ShowInfoMessage(string msg)
        {
            MessageBox.Show(msg, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
