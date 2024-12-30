using BookStore.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.UnitTests.Users
{
    internal static class UserData
    {
        public static readonly FirstName FirstName = new("fist");
        public static readonly LastName LastName = new("last");
        public static readonly Email Email = new("test@test.com");
    }
}
