namespace Shared.Contracts.Requests
{
    public sealed record UpdateGlobalAssetRequest(
        string AssetId,
        string FileName,
        string AssetName);
}