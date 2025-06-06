using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class BookingEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EventId { get; set; } = null!;
    public string CustomerId { get; set; } = null!;
    public int TicketQuantity { get; set; } = 1;
    public int PackageId { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime BookingDate { get; set; } = DateTime.Now;
}
