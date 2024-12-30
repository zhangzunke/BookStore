using BookStore.Domain.Apartments;
using BookStore.Domain.Bookings;
using BookStore.Domain.Reviews;
using BookStore.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Apartment> Apartments { get; }

        DbSet<Booking> Bookings { get; }

        DbSet<Review> Reviews { get; }

        DbSet<User> Users { get; }
    }
}
