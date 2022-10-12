using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticket.Models;

namespace User.Controllers
{
    [Route("bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingContext _context;

        public BookingController(BookingContext context)
        {
            _context = context;
        }

        // GET: bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetItems()
        {
            return await _context.Bookings
                .Select(x => BookingToDTO(x))
                .ToListAsync();
        }

        // GET: bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetItem(long id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return BookingToDTO(booking);
        }

        // PATCH: bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateItem(long id, BookingDTO bookingDTO)
        {
            if (id != bookingDTO.Id)
            {
                return BadRequest();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            booking.Title = bookingDTO.Title;
            booking.Quantity = bookingDTO.Quantity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookingDTO>> CreateItem(BookingDTO bookingDTO)
        {
            var booking = new BookingModel
            {
                Id = bookingDTO.Id,
                UserId = bookingDTO.UserId,
                TickedId = bookingDTO.TickedId,
                Title = bookingDTO.Title,
                Photo = bookingDTO.Photo,
                Description = bookingDTO.Description,
                Date = bookingDTO.Date,
                DateBooked = bookingDTO.DateBooked,
                Quantity = bookingDTO.Quantity
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetItem),
                new { id = booking.Id },
                BookingToDTO(booking));
        }

        // DELETE: bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(long id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(long id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }

        private static BookingDTO BookingToDTO(BookingModel booking) =>
            new BookingDTO
            {
                Id = booking.Id,
                UserId = booking.UserId,
                TickedId = booking.TickedId,
                Title = booking.Title,
                Photo = booking.Photo,
                Description = booking.Description,
                Date = booking.Date,
                DateBooked = booking.DateBooked,
                Quantity = booking.Quantity,
            };
    }
}