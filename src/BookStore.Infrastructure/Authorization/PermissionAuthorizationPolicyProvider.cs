﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Authorization
{
    internal sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _authorizationOptions;
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) 
            : base(options)
        {
            _authorizationOptions = options.Value;
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if(policy is not null)
            {
                return policy;
            }

            var permissionPolicy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();

            _authorizationOptions.AddPolicy(policyName, permissionPolicy);

            return permissionPolicy;
        }
    }
}
