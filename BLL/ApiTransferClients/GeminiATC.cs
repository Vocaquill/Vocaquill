using AutoMapper;
using BLL.ApiTransferModels;
using DAL.ApiClients;
using DAL.ApiModels.Additionals;

namespace BLL.ApiTransferClients
{
    public class GeminiATC
    {
        public GeminiATC() 
        {
            this._apiClient = new GeminiApiClient();
             this._mapper = new MapperConfiguration(cfg =>
             {
                 cfg.CreateMap<AiQuestionSettings, AiQuestionSettingsATD>();
                 cfg.CreateMap<AiQuestionSettingsATD, AiQuestionSettings>();
             }
             ).CreateMapper();
        }

        public async Task<string> CreateSummary(AiQuestionSettingsATD question) 
        {
            return await this._apiClient.CreateSummary(this._mapper.Map<AiQuestionSettings>(question));
        }

        private GeminiApiClient _apiClient;
        private readonly IMapper _mapper;
    }
}
