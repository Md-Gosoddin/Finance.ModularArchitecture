using Evently.Module.Consumer.Domain.Consumer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Module.Consumer.Infrastructure.Consumer;
internal sealed class CustomerConfiguration : IEntityTypeConfiguration<ConsumerEntity>
{
    public void Configure(EntityTypeBuilder<ConsumerEntity> builder)
    {
        builder.HasKey(c => c.ConsumerId);
        builder.Property(c => c.TypeofConsumer).IsRequired().HasMaxLength(200);
    }
}
