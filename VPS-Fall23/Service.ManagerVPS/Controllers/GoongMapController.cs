using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.DTO.AppSetting;
using Service.ManagerVPS.ExternalClients;

namespace Service.ManagerVPS.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GoongMapController : Controller
    {
        readonly GoongMapConfig goongMapConfig;
        readonly GoongMapRestApiService goongMapRestApiService;
        public GoongMapController(IOptions<GoongMapConfig> config)
        {
            goongMapConfig = config.Value;
            goongMapRestApiService = new GoongMapRestApiService(goongMapConfig);
        }
        [HttpGet]
        public async Task<List<DTO.GoongMap.AutoComplete.Prediction>> AutoComplete([FromQuery] DTO.GoongMap.AutoComplete.Request request)
        {
            return (await goongMapRestApiService.PlaceAutoComplete(request)).Predictions;
        }
        [HttpGet]
        public async Task<DTO.GoongMap.AutoComplete.Detail.Result?> PlaceDetail(string placeId, Guid sessionToken)
        {
            return (await goongMapRestApiService.PlaceDetail(placeId, sessionToken)).Result;
        }
        [HttpGet]
        public async Task<List<DTO.GoongMap.Geocode.ResultDTO>> GetPlaceFromLocation([FromQuery] DTO.GoongMap.Position location)
        {
            return (await goongMapRestApiService.GetPlaceFromLocation(location)).Results;
        }

    }
}
