using BookStore.Domain.Apartments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories
{
    internal sealed class ApartmentRepository : Repository<Apartment, ApartmentId>, IApartmentRepository
    {
        public ApartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
