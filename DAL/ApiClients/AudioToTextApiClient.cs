using DAL.ApiModels.Responses;
using Newtonsoft.Json;

namespace DAL.ApiClients
{
    public class AudioToTextApiClient
    {
        public AudioToTextApiClient()
        {
            this._httpClient = new HttpClient();
        }

        /// <summary>
        /// Uploads an audio file to a server for speech recognition and retrieves the recognized text.
        /// </summary>
        /// <param name="filePath">The local file path of the audio file to be uploaded.</param>
        /// <param name="serverFileName">The name under which the file will be stored on the server.</param>
        /// <returns>A string containing the transcribed text from the audio file.</returns>
        /// <exception cref="Exception">
        /// Thrown if the server returns an error response or if the response structure is invalid.
        /// </exception>
        public async Task<string> GetTextFromAudioAsync(string filePath)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(filePath);

            content.Add(new StreamContent(fileStream), "file", $"{Guid.NewGuid()}.mp3");

            HttpResponseMessage response;

            try
            {
                response = await this._httpClient.PostAsync("http://35.159.119.219:5000/api/audio/upload", content); // to be replaced by a constant in the future
            }
            catch (Exception)
            {
                throw new OverflowException("Server is overflow");
            }
            
            if (!response.IsSuccessStatusCode)
                throw new Exception("Server error");

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var outerResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);

            if (outerResponse == null || !outerResponse.ContainsKey("message"))
                throw new Exception("Invalid JSON structure");

            var innerMessage = JsonConvert.DeserializeObject<AudioPostResponse>(outerResponse["message"]);

            if (innerMessage == null)
                throw new Exception("Invalid nested JSON");

            return innerMessage.Text;
        }

        private readonly HttpClient _httpClient;
    }
}
