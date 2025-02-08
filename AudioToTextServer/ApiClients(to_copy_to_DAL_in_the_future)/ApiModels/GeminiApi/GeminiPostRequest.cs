namespace AudioToTextServer.ApiModels.GeminiApi
{
    public class GeminiPostRequest
    {
        public List<GeminiContent> Contents { get; set; }

        public GeminiPostRequest(string question)
        {
            Contents = new List<GeminiContent>
            {
                new GeminiContent
                {
                    Parts = new List<GeminiPart>
                    {
                        new GeminiPart { Text = question }
                    }
                }
            };
        }
    }
}
