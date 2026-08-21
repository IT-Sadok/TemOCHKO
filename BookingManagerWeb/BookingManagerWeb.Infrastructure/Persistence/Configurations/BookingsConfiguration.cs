using BookingManagerWeb.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingManagerWeb.Infrastructure.Persistence.Configurations;

public class BookingsConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasOne(b => b.Apartment)
            .WithMany(a => a.Bookings)
            .OnDelete(DeleteBehavior.Restrict);
    }
}