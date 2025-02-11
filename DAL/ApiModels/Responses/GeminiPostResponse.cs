using DAL.ApiModels.Additionals;

namespace DAL.ApiModels.Responses
{
    public class GeminiPostResponse
    {
        public List<GeminiCandidate> Candidates { get; set; }
    }
}
