//Dima and Petro

using BLL.ApiTransferClients;
using BLL.ApiTransferModels;
using BLL.Models;
using BLL.PDFWriter;
using System.IO;
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
                        if (!IsValidQuestion(QuestionSettings))
                            throw new Exception("Invalid question! Check all input fields");

                        FunctionalityPage.ShowPromptSettings(false);

                        FunctionalityPage.ChangeFunctionality(false);

                        string audioText = await _audioToTextATC.GetTextFromAudioAsync(_audioRecorder.SavedAudioFilePath);
                        QuestionSettings.TeacherText = audioText;

                        _aiAnswer = await _geminiATC.CreateSummaryAsync(QuestionSettings);

                        FunctionalityPage.ShowInfo(_aiAnswer);

                        FunctionalityPage.ChangeFunctionality(true);

                        _currentQuery = new QueryDTO()
                        {
                            Name = QuestionSettings.LectureTopic,
                            RequestTime = DateTime.Now.ToUniversalTime(),
                            Request = $"Create summary {QuestionSettings.LectureTopic}",
                            Response = _aiAnswer
                        };

                        await SaveQueryToDBAsync(_currentQuery);

                        DynamicDesigner.ShowInfoMessage($"Конспект сформовано, ви можете його зберегти у pdf форматі");
                    }
                    catch (Exception ex)
                    {
                        FunctionalityPage.ChangeFunctionality(true);
                        DynamicDesigner.ShowErrorMessage(ex.Message);
                    }
                });
            }
        }

        public BaseCommand CreatePDFCommand
        {
            get
            {
                return _createPDFCommand ??= new BaseCommand(async _ =>
                {
                    try
                    {

                        string directoryPath = _pdfSavePath;
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string fileName = $"{_currentQuery.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                        string filePath = Path.Combine(directoryPath, fileName);

                        CreatePDF.TextToPDF(filePath, _aiAnswer);

                        DynamicDesigner.ShowInfoMessage($"Конспект збережено у файл: {fileName}");

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

            _pdfSavePath = Path.Combine(Directory.GetCurrentDirectory(), "PDF_Summaries");
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
        private BaseCommand _createPDFCommand;

        private bool _isRecording;
        private string _pdfSavePath;
        private string _aiAnswer;

        private AudioRecorder _audioRecorder;
        private AudioToTextATC _audioToTextATC;
        private GeminiATC _geminiATC;

        private QueryDTO _currentQuery;
    }
}
