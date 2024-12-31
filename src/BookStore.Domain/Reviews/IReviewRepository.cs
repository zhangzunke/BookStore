using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Reviews
{
    public interface IReviewRepository
    {
        Task<Review?> GetByIdAsync(ReviewId id, CancellationToken cancellationToken = default);

        void Add(Review review);

        void Remove(Review review);
    }
}
