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

public class StocksDb : DbContext
{
    private readonly string _dbPath;

    public StocksDb(string dbPath)
    {
        _dbPath = dbPath;
    }

    public DbSet<Ticker> Tickers => Set<Ticker>();
    public DbSet<Price> Prices => Set<Price>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticker>(e =>
        {
            e.ToTable("Tickers");
            e.Property(p => p.TickerSymbol).HasColumnName("ticker").IsRequired();
            e.HasIndex(p => p.TickerSymbol).IsUnique();
        });

        modelBuilder.Entity<Price>(ConfigurePrice);
    }

    private static void ConfigurePrice(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Price> e)
    {
        e.ToTable("Prices");
        e.Property(p => p.PriceValue).HasColumnName("price").IsRequired();
        e.Property(p => p.Date).HasColumnName("date").IsRequired();
        e.HasIndex(p => new { p.TickerId, p.Date }).IsUnique();
    }
}


