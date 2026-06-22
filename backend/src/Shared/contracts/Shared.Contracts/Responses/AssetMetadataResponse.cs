namespace Shared.Contracts.Responses
{
    public sealed record AssetMetadataResponse(string AssetId, S3KeyResponse S3Key);
}