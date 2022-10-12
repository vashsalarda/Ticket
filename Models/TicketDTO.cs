using System;
namespace Ticket.Models
{
    public class TicketDTO
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Photo { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public int AvailableTicket { get; set; }
    }
}

