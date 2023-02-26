using Newtonsoft.Json;
using RestSharp;
using ILogger = Serilog.ILogger;

namespace asagiv.weatherai.service
{
    public class WeatherDataApiClient
    {
        #region Statics
        private const string apiParamsErrorMessage = "Must have API key and API host.";
        #endregion

        #region Fields
        private readonly ILogger? _logger;
        private readonly string? _url;
        private readonly string? _apiKey;
        private readonly string? _apiHost;
        private readonly string? _locationQuery;
        private readonly RestClient _restClient;
        #endregion

        #region Constructor
        public WeatherDataApiClient(ILogger? logger)
        {
            _logger = logger;

            _logger?.Information("Initializing Weather Data API Client.");

            _url = Environment.GetEnvironmentVariable("API_URL");
            _apiKey = Environment.GetEnvironmentVariable("API_KEY");
            _apiHost = Environment.GetEnvironmentVariable("API_HOST");
            _locationQuery = Environment.GetEnvironmentVariable("LOCATION_QUERY");

            var requestUrl = $"{_url}?q={_locationQuery}";

            _logger?.Information("Connecting to URL: {0}.", requestUrl);
            _logger?.Information("API Key: {0}.", _apiKey);
            _logger?.Information("API Host: {0}.", _apiHost);

            if (_apiKey == null || _apiHost == null)
            {
                _logger?.Fatal(apiParamsErrorMessage);
                throw new ArgumentException(apiParamsErrorMessage);
            }

            _restClient = new RestClient(requestUrl);
        }
        #endregion

        #region Methods
        public async Task<WeatherData?> RequestCurrentWeatherAsync()
        {
            _logger?.Information("Requesting Current Weather from API.");

            var request = new RestRequest
            {
                Method = Method.Get
            };

            // Add the API key, host to the request headers.
            request.AddHeader("X-RapidAPI-Key", _apiKey);
            request.AddHeader("X-RapidAPI-Host", _apiHost);

            // Get the weather data from the Web API.
            var response = await _restClient.GetAsync(request);

            if (response.IsSuccessful && response.Content is not null)
            {
                _logger?.Information("Response Successful. (Code {0}).", response.StatusCode);

                // Parse the JSON from the weather API.
                return ParseResponse(response.Content);
            }

            _logger?.Warning("Response Unsuccessful. (Code {0}).", response.StatusCode);

            return null;
        }

        private WeatherData? ParseResponse(string responseJsonString)
        {
            _logger?.Warning("Parsing JSON Response.");

            return JsonConvert.DeserializeObject<WeatherData>(responseJsonString, WeatherDataJsonConverter.Instance);
        }
        #endregion
    }
}
