using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace asagiv.weatherai
{
    public class WeatherApiClient
    {
        #region Statics
        private const string url = @"https://weatherapi-com.p.rapidapi.com/current.json";
        private const string apiKey = "7304d83c16mshc02f440bfee9512p195ad4jsnbb975d707f6f";
        private const string apiHost = "weatherapi-com.p.rapidapi.com";
        #endregion

        #region Fields
        private readonly RestClient _restClient;
        #endregion

        #region Constructor
        public WeatherApiClient(int zipCode)
        {
            var requestUrl = $"{url}?q={zipCode}";
            _restClient = new RestClient(requestUrl);
        }
        #endregion

        #region Methods
        public async Task RequestCurrentWeatherAsync()
        {
            var request = new RestRequest
            {
                Method = Method.Get
            };

            request.AddHeader("X-RapidAPI-Key", apiKey);
            request.AddHeader("X-RapidAPI-Host", apiHost);

            var response = await _restClient.GetAsync(request);

            if (response.IsSuccessful)
            {
                var responseJson = response.Content;
            }
        }
        #endregion



    }
}
