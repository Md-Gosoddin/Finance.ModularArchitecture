using Evently.Module.User.Domain.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Module.User.InfraStructure.Configuration;
internal sealed class UserConfiguration : IEntityTypeConfiguration<ClientModules>
{
    public void Configure(EntityTypeBuilder<ClientModules> builder)
    {
        builder.HasKey(u => u.ClientGuid);
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.Email);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.IdentityId);
    }
}
