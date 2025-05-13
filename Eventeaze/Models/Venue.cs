using System.ComponentModel.DataAnnotations.Schema;

namespace Eventeaze.Models
{
    public class Venue
    {
        public int VenueId { get; set; }

        public string VenueName { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
