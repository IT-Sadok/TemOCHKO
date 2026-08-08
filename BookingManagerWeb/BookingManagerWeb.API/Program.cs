using BookingManagerWeb.Application;
using BookingManagerWeb.Endpoints;
using BookingManagerWeb.Infrastructure;
using BookingManagerWeb.Infrastructure.Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapAuthorization();

app.UseHttpsRedirection();

await IdentityDataSeeder.SeedAsync(app.Services);

app.Run();
