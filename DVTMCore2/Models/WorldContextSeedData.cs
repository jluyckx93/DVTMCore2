using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTMCore2.Models
{
    public class WorldContextSeedData
    {
        private WorldContext _context;

        public WorldContextSeedData(WorldContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedData()
        {
            if (!_context.Countries.Any())
            {
                var seed = new Country()
                {
                    NameCountry = "Belgium",
                    Cities = new List<City>()
                    {
                        new City(){NameCity="Brussels", Latitude= 50.85045,Longitude=4.34878 },
                        new City(){NameCity="Antwerp", Latitude= 51.21989,Longitude=4.40346 },
                        new City(){NameCity="Liege", Latitude= 50.63373,Longitude=5.56749 },
                        new City(){NameCity="Zaventem", Latitude= 50.88365,Longitude=4.47298 }
                    }
                };

                _context.Countries.Add(seed);
                _context.Cities.AddRange(seed.Cities);


                var seed2 = new Country()
                {
                    NameCountry = "France",
                    Cities = new List<City>()
                    {
                        new City(){NameCity="Paris", Latitude= 48.85341,Longitude=2.3488 },
                        new City(){NameCity="Lyon", Latitude= 45.74846,Longitude=4.84671 },
                        new City(){NameCity="Marseille", Latitude= 43.29695,Longitude=5.38107 },
                        new City(){NameCity="Nice", Latitude= 43.70313,Longitude=7.26608 }
                    }
                };

                _context.Countries.Add(seed2);
                _context.Cities.AddRange(seed2.Cities);

                await _context.SaveChangesAsync();
            }
        }


    }
    
}
