namespace Shared.Contracts.Requests
{
    public sealed record UpdateAssetRequest(
        string AssetId, 
        string AssetName);
}