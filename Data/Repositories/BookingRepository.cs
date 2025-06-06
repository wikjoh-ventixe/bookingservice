using Data.Context;
using Data.Entities;

namespace Data.Repositories;

public class BookingRepository(BookingDbContext context) : BaseRepository<BookingEntity>(context), IBookingRepository
{
}
