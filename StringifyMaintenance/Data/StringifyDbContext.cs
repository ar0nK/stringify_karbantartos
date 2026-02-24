using Microsoft.EntityFrameworkCore;
using StringifyMaintenance.Models;

namespace StringifyMaintenance.Data;

public class StringifyDbContext : DbContext
{
    public StringifyDbContext(DbContextOptions<StringifyDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Termek> Termekek => Set<Termek>();
    public DbSet<Rendeles> Rendelesek => Set<Rendeles>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("felhasznalo");
        modelBuilder.Entity<Termek>().ToTable("termek");
        modelBuilder.Entity<Rendeles>().ToTable("rendeles");

        base.OnModelCreating(modelBuilder);
    }
}
