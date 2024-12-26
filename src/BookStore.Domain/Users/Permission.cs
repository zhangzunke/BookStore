using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Users
{
    public sealed class Permission
    {
        public static readonly Permission UsersRead = new(1, "Users:Read");
        private Permission(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; init; }
        public string Name { get; init; }
    }
}
