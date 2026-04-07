using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Data;

public class FinanceDbContext : DbContext
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Name)
                .IsRequired()
                .HasMaxLength(120);
            entity.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(180);
            entity.Property(user => user.PasswordHash)
                .IsRequired()
                .HasMaxLength(512);
            entity.HasIndex(user => user.Email)
                .IsUnique();

            entity.HasMany(user => user.Transactions)
                .WithOne(transaction => transaction.User)
                .HasForeignKey(transaction => transaction.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(category => category.Id);
            entity.Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(category => category.Type)
                .IsRequired();

            entity.HasIndex(category => new { category.Name, category.Type })
                .IsUnique();

            entity.HasMany(category => category.Transactions)
                .WithOne(transaction => transaction.Category)
                .HasForeignKey(transaction => transaction.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(transaction => transaction.Id);
            entity.Property(transaction => transaction.Description)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(transaction => transaction.Amount)
                .HasColumnType("decimal(18,2)");
            entity.Property(transaction => transaction.Date)
                .IsRequired();
            entity.Property(transaction => transaction.Type)
                .IsRequired();
        });
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                if (entry.Entity.Id == Guid.Empty)
                {
                    entry.Entity.Id = Guid.NewGuid();
                }

                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}
