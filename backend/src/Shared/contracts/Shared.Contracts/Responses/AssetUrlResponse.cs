namespace Shared.Contracts.Responses
{
    public sealed record AssetUrlResponse(string AssetId, string AssetName, string Url, List<string> ProjectIds);
}