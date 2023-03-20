using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace scheduled_service.Database.Models
{
    public class SomeTask
    {
        public Guid Id { get; init; }
        public Status Status { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class SomeTaskConfiguration : IEntityTypeConfiguration<SomeTask>
    {
        public void Configure(EntityTypeBuilder<SomeTask> builder)
        {
            builder.ToTable("tasks", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Status)
                .HasColumnName("status");

            builder.Property(x => x.LastUpdate)
                .HasColumnName("last_update");
        }
    }

    public enum Status
    {
        Created,
        Running,
        Finished
    }
}
