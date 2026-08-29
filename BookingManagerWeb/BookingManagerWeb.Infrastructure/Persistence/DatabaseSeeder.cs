using Microsoft.Extensions.DependencyInjection;

namespace BookingManagerWeb.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedDatabase(IServiceProvider serviceProvider)
    {   
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await dbContext.Database.EnsureCreatedAsync();

        if (dbContext.Apartments.Any())
        {
            //dbContext.Apartments.RemoveRange(dbContext.Apartments);
            //await dbContext.SaveChangesAsync();
            return;
        }

        var fakeApartments = DataSeeder.GetFakeApartments(10);
        
        dbContext.Apartments.AddRange(fakeApartments);
        await dbContext.SaveChangesAsync();
    }
}