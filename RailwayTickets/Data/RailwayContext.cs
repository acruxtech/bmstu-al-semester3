using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailwayTickets.Models;

namespace RailwayTickets.Data;

public class RailwayContext : DbContext
{
    public RailwayContext(DbContextOptions<RailwayContext> options) : base(options)
    {
    }

    public DbSet<Station> Stations => Set<Station>();
    public DbSet<Train> Trains => Set<Train>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(ConfigureTicket);
    }

    private static void ConfigureTicket(EntityTypeBuilder<Ticket> entity)
    {
        entity.HasOne(t => t.FromStation)
            .WithMany(s => s.DepartureTickets)
            .HasForeignKey(t => t.FromStationId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(t => t.ToStation)
            .WithMany(s => s.ArrivalTickets)
            .HasForeignKey(t => t.ToStationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

