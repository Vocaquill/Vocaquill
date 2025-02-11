namespace DAL.ApiModels.Additionals
{
    public class AiQuestionSettings
    {
        public string LectureTopic { get; set; } = String.Empty;
        public string TeacherText { get; set; } = String.Empty;
        public string Language { get; set; } = String.Empty;
        public string SummarySize { get; set; } = String.Empty;

        public override string ToString()
        {
            return $"I need to write a lecture summary. It should be plain text but formatted with specific tags. Each tag corresponds to a different style. Here is the format:\r\n\r\nP: ... - regular text\r\nH1: ... - lecture title\r\nH2: ... - subheadings\r\nEach tag must be placed at the beginning of the corresponding text block/paragraph.\r\n\r\nI am providing you with text that was generated from a professor's speech, so it may contain some inaccuracies. If something is unclear, you can skip or infer the missing parts.\r\n\r\nThe response must be in {Language}, without your own comments, explanations, or formatted text—just the plain lecture summary with the specified tags.\nLecture title: {LectureTopic};\nThe summary should be {SummarySize} in size\nTeacher's text: {TeacherText}";
        }
    }
}
