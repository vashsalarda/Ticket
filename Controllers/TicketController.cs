using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticket.Models;

namespace Ticket.Controllers
{
    [Route("tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketContext _context;

        public TicketController(TicketContext context)
        {
            _context = context;
        }

        // GET: tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetItems()
        {
            return await _context.Tickets
                .Select(x => TicketToDTO(x))
                .ToListAsync();
        }

        // GET: tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDTO>> GetItem(long id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return TicketToDTO(ticket);
        }

        // PATCH: tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateItem(long id, TicketDTO ticketDTO)
        {
            if (id != ticketDTO.Id)
            {
                return BadRequest();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Title = ticketDTO.Title;
            ticket.Photo = ticketDTO.Photo;
            ticket.Description = ticketDTO.Description;
            ticket.AvailableTicket = ticketDTO.AvailableTicket;
            ticket.Price = ticketDTO.Price;
            ticket.Date = ticketDTO.Date;

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

        // POST: tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TicketDTO>> CreateItem(TicketDTO ticketDTO)
        {
            var ticket = new TicketModel
            {
                Id = ticketDTO.Id,
                Title = ticketDTO.Title,
                Photo = ticketDTO.Photo,
                Description = ticketDTO.Description,
                AvailableTicket = ticketDTO.AvailableTicket,
                Price = ticketDTO.Price,
                Date = ticketDTO.Date
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetItem),
                new { id = ticket.Id },
                TicketToDTO(ticket));
        }

        // DELETE: tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(long id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(long id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

        private static TicketDTO TicketToDTO(TicketModel ticket) =>
            new TicketDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Photo = ticket.Photo,
                Description = ticket.Description,
                AvailableTicket = ticket.AvailableTicket,
                Price = ticket.Price,
                Date = ticket.Date
            };
    }
}