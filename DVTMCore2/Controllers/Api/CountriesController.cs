using AutoMapper;
using DVTMCore2.Models;
using DVTMCore2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTMCore2.Controllers.Api
{
    [Route("api/countries")]
    public class CountriesController : Controller
    {
        private IWorldRepository _repository;

        public CountriesController(IWorldRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetAllCountries();

                return Ok(Mapper.Map<IEnumerable<CountryViewModel>>(results));
            }
            catch(Exception ex)
            {
                //TODO LOGGING

                return BadRequest("error :" + ex);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CountryViewModel country)
        {
            if (ModelState.IsValid)
            {
                var newCountry = Mapper.Map<Country>(country);
                
                    _repository.AddCountry(newCountry);

                    if (await _repository.SaveChangesAsync())
                    {
                        return Created($"api/countries/{country.NameCountry}", Mapper.Map<CountryViewModel>(newCountry));
                    }
                    else
                    {
                        return BadRequest("Failed to save the country into the database.");
                    }
            }
            return BadRequest("Modelstate not valid");
        }               
    }
}
