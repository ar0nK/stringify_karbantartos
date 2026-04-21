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
    public DbSet<TermekKepek> TermekKepek => Set<TermekKepek>();

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
            entity.Property(e => e.Aktiv)
                  .HasColumnName("Aktiv")
                  .HasColumnType("tinyint")
                  .HasConversion<byte>();
            entity.Property(e => e.Jogosultsag)
                  .HasColumnName("Jogosultsag")
                  .HasColumnType("tinyint(3)");
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

        modelBuilder.Entity<TermekKepek>(entity =>
        {
            entity.ToTable("termek_kepek");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.TermekId).HasColumnName("TermekId");
            entity.Property(e => e.Kep1).HasColumnName("kep1").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Kep2).HasColumnName("kep2").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Kep3).HasColumnName("kep3").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Kep4).HasColumnName("kep4").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Kep5).HasColumnName("kep5").IsRequired().HasMaxLength(255);
        });

        base.OnModelCreating(modelBuilder);
    }
}