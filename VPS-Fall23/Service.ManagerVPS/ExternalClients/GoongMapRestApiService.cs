using InvoiceApi.ExternalData;
using Service.ManagerVPS.DTO.AppSetting;
namespace Service.ManagerVPS.ExternalClients
{
    public class GoongMapRestApiService
    {
        readonly RestClient restClient;
        readonly string API_KEY;
        public GoongMapRestApiService(string baseApiUrl, string apiKey)
        {
            restClient = new RestClient(baseApiUrl);
            API_KEY = apiKey;
        }
        public GoongMapRestApiService(GoongMapConfig goongMapConfig)
            : this(goongMapConfig.BaseUrl, goongMapConfig.ApiKey)
        {

        }
        public async Task<DTO.GoongMap.AutoComplete.Response> PlaceAutoComplete(DTO.GoongMap.AutoComplete.Request request)
        {

            List<string> queryParams = new List<string>()
            {
                $"api_key={API_KEY}",
                "more_compound=true",
                $"input={request.Input}",
                $"sessiontoken={request.SessionToken}"
            };
            if (request.Position != null)
            {
                queryParams.Add($"location={request.Position.Lat},{request.Position.Lng}");
            }
            string AUTO_COMPLETE_URI = $"Place/AutoComplete?{string.Join("&", queryParams)}";
            return await restClient.Get<DTO.GoongMap.AutoComplete.Response>(AUTO_COMPLETE_URI);
        }
        public async Task<DTO.GoongMap.AutoComplete.Detail.Response> PlaceDetail(string placeId, Guid sessionToken)
        {
            string URI = $"Place/Detail?api_key={API_KEY}&place_id={placeId}&sessiontoken={sessionToken}";
            return await restClient.Get<DTO.GoongMap.AutoComplete.Detail.Response>(URI);
        }
        public async Task<DTO.GoongMap.Geocode.Response> GetPlaceFromLocation(DTO.GoongMap.Position position)
        {
            string URI = $"Geocode?api_key={API_KEY}&latlng={position.Lat},{position.Lng}";
            return await restClient.Get<DTO.GoongMap.Geocode.Response>(URI);
        }


    }
}
