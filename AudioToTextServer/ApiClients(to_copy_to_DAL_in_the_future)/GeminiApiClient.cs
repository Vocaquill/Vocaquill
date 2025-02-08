using AudioToTextServer.ApiModels.GeminiApi;
using AudioToTextServer.ClientsModels;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace AudioToTextServer.ApiClients
{
    public class GeminiApiClient
    {
        private readonly HttpClient _httpClient;

        public GeminiApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> CreateSummary(AiQuestionSettings question)
        {
            var request = new GeminiPostRequest(question.ToString());

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            string json = JsonConvert.SerializeObject(request, jsonSettings);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=AIzaSyDYcdRiM7LAUcg2Ql_00boBZff2Dfmldgo", content); // to be replaced by a constant in the future

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"AI server error: {response.StatusCode}");
            }

            string responseString = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonConvert.DeserializeObject<GeminiPostResponse>(responseString);

            return geminiResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? throw new Exception("Gemini API Error");
        }
    }
}
