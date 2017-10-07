using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTMCore2.Models
{
    public class Country
    {
        public int Id { get; set; }
        public String NameCountry { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
