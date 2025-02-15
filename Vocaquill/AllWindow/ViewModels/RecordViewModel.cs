//Dima and Petro

using BLL.ApiTransferClients;
using BLL.ApiTransferModels;
using BLL.Models;
using BLL.PDFWriter;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
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

        public ObservableCollection<QueryDTO> Queries { get; set; }
        public QueryDTO SelectedQuery { get; set; }

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
                        {
                            FunctionalityPage.ChangeConvertBtState(false);
                            await _audioRecorder.StartRecordingAsync();
                        }
                        else
                        {
                            await _audioRecorder.StopRecordingAsync();
                            await Task.Delay(500);

                            DynamicDesigner.ShowInfoMessage("Лекцію запиано, тепер ви можете згенерувати конспект");

                            FunctionalityPage.ChangeConvertBtState(true);
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

                        SelectedQuery = new QueryDTO()
                        {
                            Name = Regex.Match(_aiAnswer, @"H1P:\s*(.*?)\n").Groups[1].Value,
                            RequestTime = DateTime.Now.ToUniversalTime(),
                            Request = $"Create summary {QuestionSettings.LectureTopic}",
                            Response = _aiAnswer
                        };

                        await SaveQueryToDBAsync(SelectedQuery);

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
                        if (_aiAnswer == null || String.IsNullOrEmpty(_aiAnswer))
                            throw new Exception("There is nothing to convert!");

                        string directoryPath = _pdfSavePath;
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string fileName = $"{SelectedQuery.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                        string filePath = Path.Combine(directoryPath, fileName);

                        await CreatePDF.TextToPDFAsync("Text.pdf", _aiAnswer);

                        DynamicDesigner.ShowInfoMessage($"Конспект збережено у файл: {fileName}");

                        Process.Start("explorer.exe", directoryPath);

                    }
                    catch (Exception ex)
                    {
                        FunctionalityPage.ChangeFunctionality(true);
                        DynamicDesigner.ShowErrorMessage(ex.Message);
                    }
                });
            }
        }

        public BaseCommand DownloadPDFCommand
        {
            get
            {
                return _downloadPDFCommand ??= new BaseCommand(async _ =>
                {
                    try
                    {
                        if (SelectedQuery == null)
                            throw new Exception("Select query first!");

                        string directoryPath = _pdfSavePath;
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string fileName = $"{SelectedQuery.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                        string filePath = Path.Combine(directoryPath, fileName);

                        await CreatePDF.TextToPDFAsync(filePath, SelectedQuery.Response);

                        DynamicDesigner.ShowInfoMessage($"Конспект збережено у файл: {fileName}");

                        Process.Start("explorer.exe", directoryPath);

                    }
                    catch (Exception ex)
                    {
                        FunctionalityPage.ChangeFunctionality(true);
                        DynamicDesigner.ShowErrorMessage(ex.Message);
                    }
                });
            }
        }

        public BaseCommand ConfigQuestionCommand
        {
            get
            {
                return _configQuestionCommand ??= new BaseCommand(async _ =>
                {
                    FunctionalityPage.ShowPromptSettings(true);
                });
            }
        }

        public BaseCommand ShowQuriesListCommand
        {
            get
            {
                return _showQuriesListCommand ??= new BaseCommand(async _ =>
                {
                    Queries.Clear();
                    var tempQueries = (await DBSingleton.Instance.DBService.QueryService.GetQueriesByUserIdAsync(DBSingleton.Instance.CurrentUser.Id)).ToList();

                    foreach (var item in tempQueries)
                    {
                        Queries.Add(item);
                    }

                    FunctionalityPage.ShowQueriesList(true);
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
            Queries = new ObservableCollection<QueryDTO>();

            _pdfSavePath = Path.Combine(Directory.GetCurrentDirectory(), "PDF_Summaries");
        }

        private async Task SaveQueryToDBAsync(QueryDTO query)
        {
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
        private BaseCommand _downloadPDFCommand;
        private BaseCommand _configQuestionCommand;
        private BaseCommand _showQuriesListCommand;

        private bool _isRecording;
        private string _pdfSavePath;
        private string _aiAnswer;

        private AudioRecorder _audioRecorder;
        private AudioToTextATC _audioToTextATC;
        private GeminiATC _geminiATC;
    }
}
