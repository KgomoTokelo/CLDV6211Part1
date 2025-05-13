using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Eventeaze.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int EventId { get; set; }

        public int VenueId { get; set; }
        public DateTime BookingDate { get; set; }

        [ValidateNever]
        public Event Event { get; set; }

        [ValidateNever]
        public Venue Venue { get; set; }    


    }
}
