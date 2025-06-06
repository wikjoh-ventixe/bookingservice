namespace Business.Dtos;

public class CreateBookingRequestDto
{
    public string EventId { get; set; } = null!;
    public string CustomerId { get; set; } = null!;
    public int TicketQuantity { get; set; } = 1;
    public int PackageId { get; set; }
}
