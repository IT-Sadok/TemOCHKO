using BookingManagerWeb.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingManagerWeb.Infrastructure.Persistence.Configurations;

public class BookingsConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .IsRequired();
        
        builder.Property(b => b.UserId)
            .IsRequired();
        
        builder.Property(b => b.ApartmentId)
            .IsRequired();
    }
}