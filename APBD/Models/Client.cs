using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APBD.Models
{
    public class Client
    {
        [Key]
        public int IdClient { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string Pesel { get; set; }

        public ICollection<ClientTrip> ClientTrips { get; set; }
    }
}