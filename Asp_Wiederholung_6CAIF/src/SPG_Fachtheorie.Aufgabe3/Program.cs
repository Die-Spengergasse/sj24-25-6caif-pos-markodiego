using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;
using System.Runtime.InteropServices.Marshalling;
using SPG_Fachtheorie.Aufgabe1.Services;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Service provider
        builder.Services.AddDbContext<AppointmentContext>(opt =>
        {
            opt.UseSqlite("DataSource=cash.db");
        });
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        builder.Services.AddControllers();
        builder.Services.AddScoped<PaymentService>();
        builder.Services.AddDbContext<AppointmentContext>(options =>
    options.UseSqlite("Data Source=cash.db"));


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            using (var db = scope.ServiceProvider.GetRequiredService<AppointmentContext>())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.Seed();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.MapControllers();
        app.Run();
    }
}