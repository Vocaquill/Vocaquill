//Dima and Petro

using Vocaquill.AllWindow.Additionals;
using Vocaquill.AllWindow.PageWindow;
using Vocaquill.Commands;

namespace Vocaquill.AllWindow.ViewModels
{
    public class RecordViewModel
    {
        public MainPage FunctionalityPage { get; set; }

        #region CommandsReliasation
        public BaseCommand RecordCommand 
        {
            get
            {
                return _recordCommand ??= new BaseCommand(async _ =>
                {
                    try
                    {
                        FunctionalityPage.ChangeTimerState();
                    }
                    catch (Exception ex)
                    {
                        DynamicDesigner.ShowErrorMessage(ex.Message);
                    }
                });
            }    
        }
        #endregion

        private BaseCommand _recordCommand;
    }
}
