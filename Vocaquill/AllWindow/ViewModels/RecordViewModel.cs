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

        public AiQuestionSettingsATD QuestionSettings { get; set; }

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

                            FunctionalityPage.ShowPromptSettings(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        DynamicDesigner.ShowErrorMessage(ex.Message);
                    }
                });
            }    
        }

        public BaseCommand CreateSummaryCommand
        {
            get
            {
                return _createSummaryCommand ??= new BaseCommand(async _ =>
                {
                    try
                    {
                        FunctionalityPage.ShowPromptSettings(false);

                        if (!IsValidQuestion(QuestionSettings))
                            throw new Exception("Invalid question! Check all input fields");

                        FunctionalityPage.ChangeFunctionality(false);

                        string audioText = await _audioToTextATC.GetTextFromAudioAsync(_audioRecorder.SavedAudioFilePath);
                        QuestionSettings.TeacherText = audioText;

                        string aiAnswer = await _geminiATC.CreateSummaryAsync(QuestionSettings);

                        CreatePDF.TextToPDF("Text.pdf", aiAnswer);

                        FunctionalityPage.ShowInfo(aiAnswer);

                        FunctionalityPage.ChangeFunctionality(true);

                        await SaveQueryToDBAsync(new QueryDTO() { Name = QuestionSettings.LectureTopic, RequestTime = DateTime.Now.ToUniversalTime(), Request = $"Create summary {QuestionSettings.LectureTopic}", Response = aiAnswer });
                    }
                    catch (Exception ex)
                    {
                        FunctionalityPage.ChangeFunctionality(true);
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

            QuestionSettings = new AiQuestionSettingsATD();
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

        private bool IsValidQuestion(AiQuestionSettingsATD questionSettings)
        {
            return !String.IsNullOrEmpty(questionSettings.Language) && !String.IsNullOrWhiteSpace(questionSettings.Language)
                && !String.IsNullOrEmpty(questionSettings.SummarySize) && !String.IsNullOrWhiteSpace(questionSettings.SummarySize)
                && !String.IsNullOrEmpty(questionSettings.LectureTopic) && !String.IsNullOrWhiteSpace(questionSettings.LectureTopic);
        }

        private BaseCommand _recordCommand;
        private BaseCommand _createSummaryCommand;

        private bool _isRecording;

        private AudioRecorder _audioRecorder;
        private AudioToTextATC _audioToTextATC;
        private GeminiATC _geminiATC;
    }
}
