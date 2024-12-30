using BookStore.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories
{
    internal sealed class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override void Add(User user)
        {
            foreach (var role in user.Roles)
            {
                DbContext.Attach(role);
            }

            DbContext.Add(user);
        }
    }
}
