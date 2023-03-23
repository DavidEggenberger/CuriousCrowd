using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.BuildingBlocks.AzureSpeechRecognition
{
    public class AzureSpeechAnalysisAPIClient
    {
        private readonly HttpClient httpClient;
        private readonly AzureSpeechAnalysisOptions options;
        public AzureSpeechAnalysisAPIClient(HttpClient httpClient, IOptions<AzureSpeechAnalysisOptions> options)
        {
            this.httpClient = httpClient;
            this.options = options.Value;
        }

        public async Task<string> GetTokenAsync()
        {
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", options.APIKey);
            var respnse = await httpClient.PostAsync("issuetoken", null);
            var responseString = await respnse.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
