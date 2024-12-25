using BookStore.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Authorization
{
    public class UserRoleResponse
    {
        public Guid UserId { get; init; }
        public List<Role> Roles { get; init; } = [];
    }
}
