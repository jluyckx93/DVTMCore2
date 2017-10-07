using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTMCore2.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;

        public WorldRepository(WorldContext context)
        {
            _context = context;
        }

        public void AddCity(string countryName, City newCity)
        {
            var country = GetCountryByName(countryName);

            if(country != null)
            {
                country.Cities.Add(newCity);
                _context.Cities.Add(newCity);
            }
        }

        public void AddCountry(Country country)
        {
            _context.Add(country);
        }

        public IEnumerable<Country> GetAllCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountryByName(string countryName)
        {
            return _context.Countries
                .Include(c => c.Cities)
                .Where(c => c.NameCountry == countryName)
                .FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
