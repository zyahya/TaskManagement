using Microsoft.EntityFrameworkCore;

using TaskManagement.Core.Models;

namespace TaskManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(te =>
        {
            te.Property(ti => ti.Title)
                .HasMaxLength(100);

            te.Property(ti => ti.Description)
                .HasMaxLength(200);

            te.Property(ti => ti.Status)
                .HasDefaultValue(TaskItemStatus.Pending)
                .IsRequired();
        });

        modelBuilder.Entity<User>()
            .HasMany(u => u.TaskItems)
            .WithOne(t => t.User);
    }

    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
}
