using BookStore.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Users
{
    public static class UserErrors
    {
        public static Error NotFound = new(
            "User.NotFound",
            "The user with the specified identifier was not found"
            );
        public static Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "The provided credentials were invalid");
    }
}
