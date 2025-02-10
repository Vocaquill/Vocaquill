using DAL.ApiClients;

namespace BLL.ApiTransferClients
{
    public class AudioToTextATC
    {
        public AudioToTextATC() 
        {
            this._apiClient = new AudioToTextApiClient();
        }

        public async Task<string> GetTextFromAudioAsync(string filePath, string serverFileName) 
        {
            return await this._apiClient.GetTextFromAudioAsync(filePath, serverFileName);
        }

        private AudioToTextApiClient _apiClient;
    }
}
