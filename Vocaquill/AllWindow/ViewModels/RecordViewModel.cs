//Dima and Petro

using BLL.ApiTransferClients;
using BLL.ApiTransferModels;
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
                            await _audioRecorder.StartRecordingAsync();
                        else
                        {
                            await _audioRecorder.StopRecordingAsync();

                            await Task.Delay(500);

                            FunctionalityPage.ChangeFunctionality(false);

                            string audioText = await _audioToTextATC.GetTextFromAudioAsync(_audioRecorder.SavedAudioFilePath);
                            
                            AiQuestionSettingsATD question = new AiQuestionSettingsATD() { Language = "Ukrainian", LectureTopic = "Determine automatically", TeacherText = audioText, SummarySize = "large (1-2 A4 page)" }; // In the future, allow the user to choose the theme and size himself
                            string aiAnswer = await _geminiATC.CreateSummaryAsync(question);

                            FunctionalityPage.ShowInfo(aiAnswer);

                            FunctionalityPage.ChangeFunctionality(true);
                        }
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
            _audioRecorder = new AudioRecorder();
            _audioToTextATC = new AudioToTextATC();
            _geminiATC = new GeminiATC();
        }

        private BaseCommand _recordCommand;

        private bool _isRecording;

        private AudioRecorder _audioRecorder;
        private AudioToTextATC _audioToTextATC;
        private GeminiATC _geminiATC;
    }
}
