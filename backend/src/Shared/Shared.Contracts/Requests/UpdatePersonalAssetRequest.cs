namespace Shared.Contracts.Requests
{
    public sealed record UpdatePersonalAssetRequest(
        string AssetId, 
        string UserId,
        string FileName);
}