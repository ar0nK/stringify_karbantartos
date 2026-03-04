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
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("felhasznalo");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Nev).HasColumnName("Nev");
            entity.Property(e => e.Email).HasColumnName("Email");
            entity.Property(e => e.Jelszo).HasColumnName("Jelszo");
            entity.Property(e => e.Salt).HasColumnName("SALT");
            entity.Property(e => e.Aktiv).HasColumnName("Aktiv");
            entity.Property(e => e.Jogosultsag)
                  .HasColumnName("Jogosultsag")
                  .HasColumnType("tinyint");
        });

        modelBuilder.Entity<Termek>(entity =>
        {
            entity.ToTable("termek");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Nev).HasColumnName("Nev");
            entity.Property(e => e.Leiras).HasColumnName("Leiras");
            entity.Property(e => e.RovidLeiras).HasColumnName("RovidLeiras");
            entity.Property(e => e.Ar).HasColumnName("Ar");
            entity.Property(e => e.GitarTipusId).HasColumnName("GitarTipusId");
            entity.Property(e => e.Letrehozva).HasColumnName("Letrehozva");
            entity.Property(e => e.Elerheto)
                  .HasColumnName("Elerheto")
                  .HasColumnType("tinyint");
        });

        modelBuilder.Entity<Rendeles>(entity =>
        {
            entity.ToTable("rendeles");
        });

        base.OnModelCreating(modelBuilder);
    }
}