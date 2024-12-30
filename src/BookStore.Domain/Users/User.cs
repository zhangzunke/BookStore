﻿using BookStore.Domain.Abstractions;
using BookStore.Domain.Users.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Users
{
    public sealed class User: Entity
    {
        private readonly List<Role> _roles = new();
        private User(Guid id, FirstName firstName, LastName lastName, Email email) :
            base(id) 
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        private User()
        {
        }

        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public Email Email { get; private set; }
        public string IdentityId { get; private set; } = string.Empty;
        public IReadOnlyCollection<Role> Roles => _roles.ToList();
        public static User CreateUser(FirstName firstName, LastName lastName, Email email)
        {
            var user = new User(Guid.NewGuid(), firstName, lastName, email);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

            user._roles.Add(Role.Registered);

            return user;
        }

        public void SetIdentityId(string identityId) => IdentityId = identityId;

        public void Update(FirstName firstName, LastName lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
