namespace Shared.Contracts.Requests
{
    public sealed record DeletePersonalCategoryRequest(string CategoryId, string UserId);
}