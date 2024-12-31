using BookStore.Application.Abstractions.Caching;
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
        private readonly ICacheService _cacheService;

        public AuthorizationService(ApplicationDbContext dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<UserRoleResponse> GetRolesForUserAsync(string identityId)
        {
            var cacheKey = $"auth:roles-{identityId}";
            var cachedRoles = await _cacheService.GetAsync<UserRoleResponse>(cacheKey);
            
            if (cachedRoles is not null)
            {
                return cachedRoles;
            }

            var roles = await _dbContext.Set<User>()
                .Where(u => u.IdentityId == identityId)
                .Select(u => new UserRoleResponse
                {
                    UserId = u.Id.Value,
                    Roles = u.Roles.ToList()
                })
                .FirstAsync();

            await _cacheService.SetAsync(cacheKey, roles);

            return roles;
        }

        public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
        {
            var cacheKey = $"auth:permissions-{identityId}";
            var cachedPermissions = await _cacheService.GetAsync<HashSet<string>>(cacheKey);
            if (cachedPermissions is not null)
                return cachedPermissions;

            var permissions = await _dbContext.Set<User>()
                .Where(u => u.IdentityId == identityId)
                .SelectMany(u => u.Roles.Select(r => r.Permissions))
                .FirstAsync();

            var permissionsSet = permissions.Select(p => p.Name).ToHashSet();

            await _cacheService.SetAsync(cacheKey, permissionsSet);

            return permissionsSet;
        }
    }
}
