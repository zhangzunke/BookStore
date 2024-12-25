using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Data;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Users.GetLoggedInUser
{
    internal sealed class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IUserContext _userContext;

        public GetLoggedInUserQueryHandler(
            ISqlConnectionFactory sqlConnectionFactory,
            IUserContext userContext)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _userContext = userContext;
        }

        public async Task<Result<UserResponse>> Handle(
            GetLoggedInUserQuery request, 
            CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT Id, FirstName, LastName FROM Users WHERE IdentityId = @IdentityId
                """;

            var user = await connection.QuerySingleAsync<UserResponse>(sql, new { _userContext.IdentityId });

            return user;
        }
    }
}
