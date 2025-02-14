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
            return $"Generate a structured lecture summary using the specified format. The text should not contain any additional formatting, only tags.\n\n" +
                   "## Available Tags:\n\n" +
                   "**Heading Tags (Font Sizes):**\n" +
                   "H1 - 24pt (Main topic)\n" +
                   "H3 - 20pt (Main sections)\n" +
                   "H5 - 16pt (Subsections)\n" +
                   "H7 - Paragraph (Explanatory text)\n\n" +
                   "**Text Formatting Tags (Used only in paragraphs):**\n" +
                   "B: - Bold, I: - Italic, U: - Underline, S: - Strikethrough (These tags cannot be placed in the middle of a sentence, only as an addition to H1, H3, for example H1BP)\n\n" +
                   "**Expected Structure:**\n" +
                   "- `H1P: Lecture Title`\n" +
                   "- `H3P: Section 1`\n" +
                   "- `H5P: Subsection 1.1`\n" +
                   "- `P: Explanation text.`\n" +
                   "- `H5P: Subsection 1.2`\n" +
                   "- `P: Explanation text.`\n" +
                   "- `H3P: Section 2`\n" +
                   "- `P: General explanation.`\n\n" +
                   "Each heading should introduce a new section. The summary must always have at least one H1 title, multiple H2 sections, and accompanying P paragraphs.\n" +
                   "Do not generate summaries that contain only headings or only paragraphs.\n\n" +
                   "**Note:** The teacher's text was converted from audio, so there may be some inaccuracies. If a part of the text is unclear, you may skip or infer the missing information.\n\n" +
                   $"Lecture title: {LectureTopic}\n" +
                   $"Summary size: {SummarySize}\n" +
                   $"Language: {Language}\n" +
                   $"Teacher's text (converted from audio, may contain errors):\n{TeacherText}";
        }

    }
}
