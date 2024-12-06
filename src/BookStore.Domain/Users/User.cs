using BookStore.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Users
{
    public sealed class User(
        Guid id, 
        FirstName firstName, 
        LastName lastName, 
        Email email) : Entity(id)
    {
        public FirstName FirstName { get; private set; } = firstName;
        public LastName LastName { get; private set; } = lastName;
        public Email Email { get; private set; } = email;
    }
}
