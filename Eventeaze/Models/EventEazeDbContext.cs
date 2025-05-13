using Microsoft.EntityFrameworkCore;
using Eventeaze.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eventeaze.Models
{
    public class EventEazeDbContext: DbContext
    {
        public EventEazeDbContext(DbContextOptions<EventEazeDbContext> options) : base(options)
        {

        }
        public DbSet<Eventeaze.Models.Venue> Venue { get; set; } = default!;
 
        public DbSet<Event> Event { get; set; }
        public DbSet<Booking> Booking { get; set; }
    }
}
