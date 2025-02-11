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
                        _isRecording = !_isRecording;
                        FunctionalityPage.ChangeTimerState();

                        if (_isRecording)
                            _audioRecorder.StartRecording();
                        else
                            _audioRecorder.StopRecording();
                    }
                    catch (Exception ex)
                    {
                        DynamicDesigner.ShowErrorMessage(ex.Message);
                    }
                });
            }    
        }
        #endregion

        public RecordViewModel() 
        {
            this._audioRecorder = new AudioRecorder();
        }

        private BaseCommand _recordCommand;

        private bool _isRecording;
        private AudioRecorder _audioRecorder;
    }
}
