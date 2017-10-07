using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVTMCore2.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Country> GetAllCountries();

        Country GetCountryByName(string countryName);

        Task<bool> SaveChangesAsync();

        void AddCountry(Country Country);
        void AddCity(string countryName, City newCity);
    }
}