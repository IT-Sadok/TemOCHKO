using BookingManagerWeb.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingManagerWeb.Infrastructure.Persistence.Configurations;

public class ApartmentsConfiguration : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasMany(a => a.Bookings)
            .WithOne(b => b.Apartment);
    }
}