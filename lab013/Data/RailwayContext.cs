using Microsoft.EntityFrameworkCore;
using lab013.Models;

namespace lab013.Data;

public class RailwayContext : DbContext
{
	public DbSet<Train> Trains { get; set; }
	public DbSet<Route> Routes { get; set; }
	public DbSet<Passenger> Passengers { get; set; }
	public DbSet<Ticket> Tickets { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite("Data Source=railway.db");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		
		// Настройка связей
		modelBuilder.Entity<Route>()
			.HasOne(r => r.Train)
			.WithMany()
			.HasForeignKey(r => r.TrainId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Ticket>()
			.HasOne(t => t.Route)
			.WithMany()
			.HasForeignKey(t => t.RouteId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Ticket>()
			.HasOne(t => t.Passenger)
			.WithMany()
			.HasForeignKey(t => t.PassengerId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}

