using System;

namespace Ticket.Models
{
    public class BookingDTO
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public string? TickedId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateBooked { get; set; }
        public int Quantity { get; set; }
    }
}