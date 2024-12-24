using BookStore.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore.Application.Users.LoginUser
{
    public sealed record LoginUserCommand(string Email, string Password) : ICommand<AccessTokenResponse>;
}
