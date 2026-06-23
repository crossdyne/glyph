namespace Shared.Contracts.Assets.Responses
{
    public sealed record AssetMetadataResponse(string AssetId, string AssetName, S3KeyResponse S3Key, string CategoryId, List<string> ProjectIds);
}