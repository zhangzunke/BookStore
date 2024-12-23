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
        public static User CreateUser(FirstName firstName, LastName lastName, Email email)
        {
            var user = new User(Guid.NewGuid(), firstName, lastName, email);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
            return user;
        }
    }
}
