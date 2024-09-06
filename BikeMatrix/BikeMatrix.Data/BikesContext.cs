using BikeMatrix.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeData;

public class BikesContext
    : DbContext
{
    public DbSet<Bike> Bikes { get; set; }

    public string DbPath { get; }

    public BikesContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "bikes.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}
