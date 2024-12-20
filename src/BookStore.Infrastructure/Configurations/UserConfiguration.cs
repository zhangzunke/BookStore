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
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(t => t.Id);

            builder.Property(x => x.FirstName)
                .HasMaxLength(200)
                .HasConversion(firstName => firstName.Value, value => new FirstName(value));

            builder.Property(x => x.LastName)
                .HasMaxLength(200)
                .HasConversion(lastName => lastName.Value, value => new LastName(value));

            builder.Property(x => x.Email)
                .HasMaxLength(400)
                .HasConversion(email => email.Value, value => new Domain.Users.Email(value));

            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
