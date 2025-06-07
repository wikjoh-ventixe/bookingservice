using Business.Dtos;
using Business.Dtos.API;
using Business.Models;

namespace Business.Interfaces;

public interface IBookingService
{
    Task<BookingResult<Booking?>> CreateBookingAsync(CreateBookingRequestDto request);
    Task<BookingResult<bool>> DeleteEventAsync(string id);
    Task<BookingResult<IEnumerable<Booking>>> GetAllBookingsAsync();
    Task<BookingResult<IEnumerable<EventTicketsSold>>> GetTicketsSoldAmountAllEvents();
    Task<BookingResult<int>> GetTicketsSoldAmountByEventId(string eventId);
}
