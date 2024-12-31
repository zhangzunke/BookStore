using BookStore.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories
{
    internal abstract class Repository<TEntity, TEntityId> 
        where TEntity : Entity<TEntityId>
        where TEntityId : class
    {
        protected readonly ApplicationDbContext DbContext;
        protected Repository(ApplicationDbContext dbContext) 
        {
            DbContext = dbContext;
        }

        public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual void Add(TEntity entity)
        {
            DbContext.Add(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            DbContext.Remove(entity);
        }
    }
}
