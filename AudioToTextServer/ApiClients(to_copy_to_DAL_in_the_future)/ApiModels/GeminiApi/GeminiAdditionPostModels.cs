namespace AudioToTextServer.ApiModels.GeminiApi
{
    public class GeminiContent
    {
        public List<GeminiPart> Parts { get; set; }
    }

    public class GeminiPart
    {
        public string Text { get; set; }
    }

    public class GeminiCandidate
    {
        public GeminiContent Content { get; set; }
    }
}
