using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTMCore2.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        public String Name  { get; set; }
        [Required]
        [EmailAddress]
        public String Email { get; set; }
        [Required]
        [StringLength(4096, MinimumLength = 10)]
        public String Message { get; set; }
    }
}
