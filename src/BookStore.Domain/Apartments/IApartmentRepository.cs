using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Apartments
{
    public interface IApartmentRepository
    {
        Task<Apartment?> GetByIdAsync(ApartmentId id, CancellationToken cancellationToken = default);
        void Add(Apartment apartment);
    }
}
