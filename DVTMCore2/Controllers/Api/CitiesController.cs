using AutoMapper;
using DVTMCore2.Models;
using DVTMCore2.Services;
using DVTMCore2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DVTMCore2.Controllers.Api
{
    [Route("/api/countries/{countryName}/cities")]
    public class CitiesController : Controller
    {
        private IWorldRepository _repository;
        private GeoCoordsService _coordsService;

        public CitiesController(IWorldRepository repository, GeoCoordsService coordsService)
        {
            _repository = repository;
            _coordsService = coordsService;
        }

        [HttpGet("")]
        public IActionResult Get(string countryName)
        {
            try
            {
                var country = _repository.GetCountryByName(countryName);

                return Ok(Mapper.Map<IEnumerable<CityViewModel>>(country.Cities.OrderBy(c => c.NameCity).ToList()));
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to get the cities." + ex);
            }

           // return BadRequest("Failed to get country and cities");
        }


        [HttpPost("")]
        public async Task<IActionResult> Post(string countryName, [FromBody]CityViewModel cityVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newCity = Mapper.Map<City>(cityVM);

                    var result = await _coordsService.GetCoordsAsync(newCity.NameCity, countryName);

                    if (!result.Success)
                    {
                        return BadRequest("Error");
                    }
                    else
                    {
                        newCity.Latitude = result.Latitude;
                        newCity.Longitude = result.Longitude;
                        _repository.AddCity(countryName, newCity);
                        if (await _repository.SaveChangesAsync())
                        {
                            return Created($"/api/countries/{countryName}/cities/{newCity.NameCity}",
                                Mapper.Map<CityViewModel>(newCity));
                        }
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }

                }
            
            catch (Exception ex)
            {
                return BadRequest(ex);
                //TODO LOGGING
            }

            return BadRequest("Failed to add city.");  
        }
    }
}
