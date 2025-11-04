using Microsoft.EntityFrameworkCore;

using TaskManagement.Core.Models;

namespace TaskManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    DbSet<TaskItem> Tasks { get; set; }
}
