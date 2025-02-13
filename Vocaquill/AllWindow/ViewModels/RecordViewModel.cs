//Dima and Petro

using BLL.ApiTransferClients;
using BLL.ApiTransferModels;
using BLL.Models;
using BLL.PDFWriter;
using Vocaquill.AllWindow.Additionals;
using Vocaquill.AllWindow.PageWindow;
using Vocaquill.Commands;
using Vocaquill.Singleton;

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
                            //FunctionalityPage.ShowRequestPopup(true);

                            await _audioRecorder.StopRecordingAsync();
                            await Task.Delay(500);

                            FunctionalityPage.ChangeFunctionality(false);

                            string audioText = await _audioToTextATC.GetTextFromAudioAsync(_audioRecorder.SavedAudioFilePath);
                            
                            AiQuestionSettingsATD question = new AiQuestionSettingsATD() { Language = "Ukrainian", LectureTopic = "Determine automatically", TeacherText = audioText, SummarySize = "large (1-2 A4 page)" }; // In the future, allow the user to choose the theme and size himself
                            string aiAnswer = await _geminiATC.CreateSummaryAsync(question);

                            CreatePDF.TextToPDF("Text.pdf", aiAnswer);

                            FunctionalityPage.ShowInfo(aiAnswer);

                            FunctionalityPage.ChangeFunctionality(true);

                            await SaveQueryToDBAsync(new QueryDTO() { Name = question.LectureTopic, RequestTime = DateTime.Now.ToUniversalTime(), Request = question.ToString(), Response = aiAnswer });
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

        private async Task SaveQueryToDBAsync(QueryDTO query)
        {
            // temp cod for testing
            UserDTO userDTO = new UserDTO()
            {
                Login = "PetroUser",
                Password = "SecurePass123",
                Name = "Петро Тимчук",
                Email = "petro.timchuk@example.com"
            };

            //await DBSingleton.Instance.DBService.UserService.AddUserAsync(userDTO);
            DBSingleton.Instance.CurrentUser = await DBSingleton.Instance.DBService.UserService.GetUserByLoginAndPasswordAsync(userDTO.Login, userDTO.Password);
            // end testing code

            query.UserId = DBSingleton.Instance.CurrentUser.Id;
            await DBSingleton.Instance.DBService.QueryService.AddQueryAsync(query);
        }

        private BaseCommand _recordCommand;

        private bool _isRecording;

        private AudioRecorder _audioRecorder;
        private AudioToTextATC _audioToTextATC;
        private GeminiATC _geminiATC;
    }
}
