using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models;

public class Booking
{
    public string Id { get; set; } = null!;
    public string EventId { get; set; } = null!;
    public string CustomerId { get; set; } = null!;
    public int TicketQuantity { get; set; } = 1;
    public int PackageId { get; set; }

    public DateTime BookingDate { get; set; }
}
