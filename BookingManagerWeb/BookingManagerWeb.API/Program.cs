using BookingManagerWeb.Application;
using BookingManagerWeb.Application.Auth;
using BookingManagerWeb.Endpoints;
using BookingManagerWeb.Extensions;
using BookingManagerWeb.Infrastructure;
using BookingManagerWeb.Infrastructure.Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandlers();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapAuthorization();

app.UseHttpsRedirection();

await IdentityDataSeeder.SeedAsync(app.Services);

app.MapGet("/yo", () =>
{
    throw new InvalidCastException("Something went wrong");
});

app.Run();
