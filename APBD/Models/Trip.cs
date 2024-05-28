using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APBD.Models
{
    public class Trip
    {
        [Key]
        public int IdTrip { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [Required]
        public int MaxPeople { get; set; }

        public ICollection<CountryTrip> CountryTrips { get; set; }
        public ICollection<ClientTrip> ClientTrips { get; set; }
    }
}