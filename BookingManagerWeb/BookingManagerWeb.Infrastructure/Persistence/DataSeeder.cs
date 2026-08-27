using Bogus;
using BookingManagerWeb.Domain.Models;

namespace BookingManagerWeb.Infrastructure.Persistence;

public static class DataSeeder
{
    public static List<Apartment> GetFakeApartments(int count)
    {
        int i = 0;
        var apartmentFaker = new Faker<Apartment>()
            .RuleFor(o => o.Id, Guid.NewGuid)
            .RuleFor(o => o.Name, f => f.Name.JobArea())
            .RuleFor(o => o.IsActive, f => true)
            .RuleFor(o => o.PricePerNight, f => f.Finance.Amount(50, 500))
            .RuleFor(o => o.MaxGuests, f => f.Random.Int(1, 8))
            .RuleFor(o => o.Bookings, f => []);

        return apartmentFaker.Generate(count);
    }
}