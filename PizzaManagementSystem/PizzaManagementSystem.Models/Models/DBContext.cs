using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PizzaManagementSystem.Models.Models;

public partial class DBContext : IdentityDbContext<User>
{
    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Impiegato> Impiegatos { get; set; }

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
        base.OnModelCreating(modelBuilder); // for ASP Identity

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

        modelBuilder.Entity<Impiegato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Impiegat__3214EC070141A63D");

            entity.ToTable("Impiegato");

            entity.Property(e => e.Firstname).HasMaxLength(80);
            entity.Property(e => e.Surname).HasMaxLength(80);

            entity.HasOne(d => d.Area).WithMany(p => p.Impiegatos)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Impiegato__AreaI__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
