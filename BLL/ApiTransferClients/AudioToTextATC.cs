using DAL.ApiClients;

namespace BLL.ApiTransferClients
{
    public class AudioToTextATC
    {
        public AudioToTextATC() 
        {
            this._apiClient = new AudioToTextApiClient();
        }

        public async Task<string> GetTextFromAudioAsync(string filePath) 
        {
            return await this._apiClient.GetTextFromAudioAsync(filePath);
        }

        private AudioToTextApiClient _apiClient;
    }
}
