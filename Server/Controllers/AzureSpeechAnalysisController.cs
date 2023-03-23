using Client.BuildingBlocks.AzureSpeechRecognition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureSpeechAnalysisController : ControllerBase
    {
        private readonly AzureSpeechAnalysisAPIClient azureSpeechAnalysisAPIClient;

        public AzureSpeechAnalysisController(AzureSpeechAnalysisAPIClient azureSpeechAnalysisAPIClient)
        {
            this.azureSpeechAnalysisAPIClient = azureSpeechAnalysisAPIClient;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<AzureCognitiveServicesTokenDTO> GetTokenAsync()
        {
            var token = await azureSpeechAnalysisAPIClient.GetTokenAsync();
            return new AzureCognitiveServicesTokenDTO
            {
                Token = token,
            };
        }
    }
}
