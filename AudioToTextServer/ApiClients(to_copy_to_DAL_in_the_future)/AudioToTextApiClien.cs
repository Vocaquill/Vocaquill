using AudioToTextServer.ApiModels;
using Newtonsoft.Json;

namespace AudioToTextServer.ApiClients
{
    public class AudioToTextApiClien
    {
        public AudioToTextApiClien() 
        {
            this._httpClient = new HttpClient();
        }

        public async Task<string> GetTextFromAudioAsync(string filePath, string serverFileName)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(filePath);

            content.Add(new StreamContent(fileStream), "file", $"{serverFileName}.mp3");

            var response = await _httpClient.PostAsync("http://35.159.119.219:5000/api/audio/upload", content);

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

        private HttpClient _httpClient;
    }
}
