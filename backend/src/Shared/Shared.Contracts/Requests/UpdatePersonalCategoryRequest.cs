namespace Shared.Contracts.Requests
{
    public sealed record UpdatePersonalCategoryRequest(string CategoryId, string UserId, string Name);
}