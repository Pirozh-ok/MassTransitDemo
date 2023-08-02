﻿namespace GenerateTicket.Models
{
    public class TicketInfo
    {
        public string TicketId { get; set; }
        public string Email { get; set; }
        public string TicketNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
