using Microsoft.EntityFrameworkCore;
// ReSharper disable StringLiteralTypo

namespace PizzaManagementSystem.Models.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            throw new ApplicationException("Stringa di connessione non configurata");
        }

        // Abilito il log dei dati nelle query nella console
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__Area__70B820484C59AD6F");

            entity.ToTable("Area");

            entity.Property(e => e.Description).HasMaxLength(1024);
            entity.Property(e => e.InsertBy).HasMaxLength(50);
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(60);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
