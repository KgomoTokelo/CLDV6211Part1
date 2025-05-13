using Eventeaze.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eventeaze.Controllers
{
    public class BookingController : Controller
    {
        private readonly EventEazeDbContext _context;

        public BookingController(EventEazeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(String Search)

        {

            var bookings = _context.Booking
         .Include(b => b.Event)
         .Include(b => b.Venue)
         .AsQueryable();

            if (!string.IsNullOrEmpty(Search))
            {
                bookings = bookings.Where(b =>
                    b.Venue.VenueName.Contains(Search) ||
                    b.Event.EventName.Contains(Search));
            }

            return View(await bookings.ToListAsync());
        }
        // GET: Create (Async)
        public async Task<IActionResult> Create()
        {
            // Use async for consistency
            var events = await _context.Event.ToListAsync();
            var venues = await _context.Venue.ToListAsync();

            ViewBag.EventId = new SelectList(events, "EventId", "EventName");
            ViewBag.VenueId = new SelectList(venues, "VenueId", "VenueName");

            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            // Correctly fetch the event by EventId (not VenueId)
            var selectedEvent = await _context.Event
                .FirstOrDefaultAsync(e => e.EventId == booking.EventId);

            if (selectedEvent == null)
            {
                ModelState.AddModelError("", "Selected event not found.");
                // Repopulate dropdowns with SelectList
                await RepopulateDropdowns();
                return View(booking);
            }

            // Check for venue-date conflicts
            var conflict = await _context.Booking
                .AnyAsync(b => b.VenueId == booking.VenueId &&
                               b.BookingDate.Date == booking.BookingDate.Date);

            if (conflict)
            {
                ModelState.AddModelError("", "This venue is already booked for that date.");
                await RepopulateDropdowns();
                return View(booking);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking created successfully."; // Fixed typo
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Database error: {ex.InnerException?.Message}");
                    ModelState.AddModelError("", "An error occurred while saving.");
                    await RepopulateDropdowns();
                    return View(booking);
                }
            }

            // Repopulate on validation failure
            await RepopulateDropdowns();
            return View(booking);
        }

        // Helper method to repopulate dropdowns
        private async Task RepopulateDropdowns()
        {
            ViewBag.EventId = new SelectList(await _context.Event.ToListAsync(), "EventId", "EventName");
            ViewBag.VenueId = new SelectList(await _context.Venue.ToListAsync(), "VenueId", "VenueName");
        }

        public async Task<IActionResult> Details(int? id)
        {

            var book = await _context.Booking.FirstOrDefaultAsync(m => m.BookingId == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            var book = await _context.Booking.FirstOrDefaultAsync(m => m.BookingId == id);


            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool CompanyExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Booking.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }

            return View(book);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Booking book)
        {
            if (id != book.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(book.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

    }
}
