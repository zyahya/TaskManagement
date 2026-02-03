using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TaskManagement.Domain.Models;

namespace TaskManagement.Infrastructure.EntityConfigurations;

public class TaskItemConfigurations : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.Property(ti => ti.Title)
            .HasMaxLength(100);

        builder.Property(ti => ti.Description)
            .HasMaxLength(200);

        builder.Property(ti => ti.Status)
            .HasDefaultValue(TaskItemStatus.Pending)
            .IsRequired();
    }
}
