using BookStore.Application.Users.GetLoggedInUser;
using BookStore.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Authorization
{
    internal sealed class AuthorizationService
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorizationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserRoleResponse> GetRolesForUserAsync(string identityId)
        {
            var roles = await _dbContext.Set<User>()
                .Where(u => u.IdentityId == identityId)
                .Select(u => new UserRoleResponse
                {
                    UserId = u.Id,
                    Roles = u.Roles.ToList()
                })
                .FirstAsync();
            return roles;
        }

        public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
        {
            var permissions = await _dbContext.Set<User>()
                .Where(u => u.IdentityId == identityId)
                .SelectMany(u => u.Roles.Select(r => r.Permissions))
                .FirstAsync();

            return permissions.Select(p => p.Name).ToHashSet();
        }
    }
}
