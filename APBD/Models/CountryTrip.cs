using System.ComponentModel.DataAnnotations;

namespace APBD.Models
{
    public class CountryTrip
    {
        [Required]
        public int IdCountry { get; set; }
        [Required]
        public int IdTrip { get; set; }

        public Country Country { get; set; }
        public Trip Trip { get; set; }
    }
}