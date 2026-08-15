namespace Shared.Contracts.Assets.Responses
{
    public sealed record AssetUrlResponse(string AssetId, string AssetName, string Url, string CategoryId, List<string> ProjectIds, bool IsPublic);
}