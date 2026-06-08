namespace Shared.Contracts.Requests
{
    public sealed record CreatePersonalCategoryRequest(string UserId, string Name);
}