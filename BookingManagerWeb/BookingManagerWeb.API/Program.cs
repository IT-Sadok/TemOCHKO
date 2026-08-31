using BookingManagerWeb.Application;
using BookingManagerWeb.Endpoints;
using BookingManagerWeb.Extensions;
using BookingManagerWeb.Infrastructure;
using BookingManagerWeb.Infrastructure.Identity;
using BookingManagerWeb.Infrastructure.Persistence;
using BookingManagerWeb.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandlers();
builder.Services.AddJwtConfiguration(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

var api = app.MapGroup("")
    .AddEndpointFilter<ValidationMiddleware>();

api.MapAuthorization();
api.MapApartmentEndpoints();
api.MapBookingsEndpoint();

app.UseHttpsRedirection();

await IdentityDataSeeder.SeedAsync(app.Services);
await DatabaseSeeder.SeedDatabase(app.Services);

app.UseAuthentication();
app.UseAuthorization();

app.Run();
