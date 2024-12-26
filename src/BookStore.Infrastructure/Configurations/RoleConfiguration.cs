using BookStore.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(x => x.Id);

            builder.HasMany(role => role.Users)
               .WithMany(u => u.Roles)
               .UsingEntity("UserRoles");

            builder.HasMany(role => role.Permissions)
                .WithMany()
                .UsingEntity<RolePermission>();

            builder.HasData(Role.Registered);
        }
    }
}
