using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DVTMCore2.Services
{
    public class GeoCoordsService
    {
        private IConfigurationRoot _config;

        public GeoCoordsService(IConfigurationRoot config)
        {
            _config = config;
        }

        public async Task<GeoCoordsResult> GetCoordsAsync(string nameCity, string nameCountry)
        {
            var result = new GeoCoordsResult()
            {
                Success = false,
                Message = "Failed to get the coordinates"
            };
            var name = nameCity + ", " + nameCountry;
            var apiKey = _config["Keys:BingMaps"];
            var encodedName = WebUtility.UrlEncode(name);
            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={apiKey}";

            var client = new HttpClient();
            var json = await client.GetStringAsync(url);

            // Read out the results
            // Fragile, might need to change if the Bing API changes
            var results = JObject.Parse(json);
            var locality = (string)results["resourceSets"][0]["resources"][0]["address"]["locality"];
            var country = (string)results["resourceSets"][0]["resources"][0]["address"]["countryRegion"];
            var resources = results["resourceSets"][0]["resources"];
            if (!resources.HasValues || locality == null || country == null)
            {
                result.Message = $"Could not find '{name}' as a location";
            }
            else
            {
                Console.WriteLine(resources);
                var confidence = (string)resources[0]["confidence"];
                var testName = (string)resources[0]["name"];

                bool booleanTest = (confidence == "High" && locality == nameCity && country == nameCountry);

                if (booleanTest)
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    var lat = (double)coords[0];
                    var longitude = (double)coords[1];
                    result.Latitude = Math.Round(lat,5);
                    result.Longitude = Math.Round(longitude, 5);
                    result.Success = true;
                    result.Message = "Success";
                }
                else
                {
                    result.Message = $"Could not find a confident match for '{name}' as a location";
                }
            }

            return result;
        }
    }
}
