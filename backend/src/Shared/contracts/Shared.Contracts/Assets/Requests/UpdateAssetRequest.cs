namespace Shared.Contracts.Assets.Requests
{
    public sealed record UpdateAssetRequest(
        string AssetId, 
        string AssetName,
        string CategoryId);
}