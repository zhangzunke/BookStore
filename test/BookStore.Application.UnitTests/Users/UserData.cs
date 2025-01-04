﻿using BookStore.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.UnitTests.Users
{
    internal static class UserData
    {
        public static User Create() => User.Create(FirstName, LastName, Email);

        public static readonly FirstName FirstName = new("First");
        public static readonly LastName LastName = new("Last");
        public static readonly Email Email = new("test@test.com");
    }
}
