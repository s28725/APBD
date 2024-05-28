using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APBD.Models
{
    public class Country
    {
        [Key]
        public int IdCountry { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<CountryTrip> CountryTrips { get; set; }
    }
}