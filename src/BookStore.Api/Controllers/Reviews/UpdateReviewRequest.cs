namespace BookStore.Api.Controllers.Reviews
{
    public sealed record UpdateReviewRequest(int Rating, string Comment);
}
