using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTMCore2.ViewModels
{
    public class CountryViewModel
    {
        [Required]
        public string NameCountry { get; set; }
    }
}
