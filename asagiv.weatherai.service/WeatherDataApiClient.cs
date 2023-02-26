using Newtonsoft.Json;
using RestSharp;

namespace asagiv.weatherai.service
{
    public class WeatherDataApiClient
    {
        #region Statics
        private const string apiParamsErrorMessage = "Must have API key and API host.";
        #endregion

        #region Fields
        private readonly string? _url;
        private readonly string? _apiKey;
        private readonly string? _apiHost;
        private readonly string? _locationQuery;
        private readonly RestClient _restClient;
        #endregion

        #region Constructor
        public WeatherDataApiClient()
        {
            _url = Environment.GetEnvironmentVariable("API_URL");
            _apiKey = Environment.GetEnvironmentVariable("API_KEY");
            _apiHost = Environment.GetEnvironmentVariable("API_HOST");
            _locationQuery = Environment.GetEnvironmentVariable("LOCATION_QUERY");

            if(_apiKey == null || _apiHost == null)
            {
                throw new ArgumentException(apiParamsErrorMessage);
            }

            var requestUrl = $"{_url}?q={_locationQuery}";

            _restClient = new RestClient(requestUrl);
        }
        #endregion

        #region Methods
        public async Task<WeatherData?> RequestCurrentWeatherAsync()
        {
            var request = new RestRequest
            {
                Method = Method.Get
            };

            request.AddHeader("X-RapidAPI-Key", _apiKey);
            request.AddHeader("X-RapidAPI-Host", _apiHost);

            var response = await _restClient.GetAsync(request);

            if (response.IsSuccessful && response.Content is not null)
            {
                return ParseResponse(response.Content);
            }

            return null;
        }

        private static WeatherData? ParseResponse(string responseJsonString)
        {
            return JsonConvert.DeserializeObject<WeatherData>(responseJsonString, WeatherDataJsonConverter.Instance);
        }
        #endregion
    }
}
