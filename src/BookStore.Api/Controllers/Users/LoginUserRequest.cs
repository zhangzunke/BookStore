namespace BookStore.Api.Controllers.Users
{
    public sealed record LoginUserRequest(string Email, string Password);
}
