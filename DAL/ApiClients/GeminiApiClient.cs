using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using DAL.ApiModels.Additionals;
using DAL.ApiModels.Requests;
using DAL.ApiModels.Responses;

namespace DAL.ApiClients
{
    public class GeminiApiClient
    {
        public GeminiApiClient()
        {
            this._httpClient = new HttpClient();
        }

        /// <summary>
        /// Sends a request to the Gemini AI API to generate a content summary based on the provided question settings.
        /// </summary>
        /// <param name="question">The settings for the AI-generated question, which will be used to create the request.</param>
        /// <returns>A string containing the AI-generated summary.</returns>
        /// <exception cref="Exception">
        /// Thrown if the AI server returns an error response or if the response structure is invalid.
        /// </exception>
        public async Task<string> CreateSummaryAsync(AiQuestionSettings question)
        {
            var request = new GeminiPostRequest(question.ToString());

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            string json = JsonConvert.SerializeObject(request, jsonSettings);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this._httpClient.PostAsync("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=AIzaSyDYcdRiM7LAUcg2Ql_00boBZff2Dfmldgo", content); // to be replaced by a constant in the future

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"AI server error: {response.StatusCode}");
            }

            string responseString = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonConvert.DeserializeObject<GeminiPostResponse>(responseString);

            return geminiResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? throw new Exception("Gemini API Error");
        }

        private readonly HttpClient _httpClient;
    }
}
