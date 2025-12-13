using Microsoft.EntityFrameworkCore;

public class Ticker
{
    public int Id { get; set; }
    public string TickerSymbol { get; set; } = string.Empty;
}

public class Price
{
    public int Id { get; set; }
    public int TickerId { get; set; }
    public double PriceValue { get; set; }
    public string Date { get; set; } = string.Empty; 
}

public class TodaysCondition
{
    public int Id { get; set; }
    public int TickerId { get; set; }
    public string State { get; set; } = string.Empty;
    public string UpdatedAt { get; set; } = string.Empty; 
}

public class StocksDb : DbContext
{
    public DbSet<Ticker> Tickers => Set<Ticker>();
    public DbSet<Price> Prices => Set<Price>();
    public DbSet<TodaysCondition> TodaysConditions => Set<TodaysCondition>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "stocks.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticker>(e =>
        {
            e.ToTable("Tickers");
            e.Property(p => p.TickerSymbol).HasColumnName("ticker").IsRequired();
            e.HasIndex(p => p.TickerSymbol).IsUnique();
        });

        modelBuilder.Entity<Price>(e =>
        {
            e.ToTable("Prices");
            e.Property(p => p.PriceValue).HasColumnName("price").IsRequired();
            e.Property(p => p.Date).HasColumnName("date").IsRequired();
            e.HasIndex(p => new { p.TickerId, p.Date }).IsUnique();
        });

        modelBuilder.Entity<TodaysCondition>(e =>
        {
            e.ToTable("TodaysCondition");
            e.Property(p => p.State).HasColumnName("state").IsRequired();
            e.Property(p => p.UpdatedAt).HasColumnName("updated_at").IsRequired();
            e.HasIndex(p => p.TickerId).IsUnique();
        });
    }
}


