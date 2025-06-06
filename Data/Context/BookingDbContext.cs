using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class BookingDbContext(DbContextOptions<BookingDbContext> options) : DbContext(options)
{
    public DbSet<BookingEntity> Bookings { get; set; }
}
