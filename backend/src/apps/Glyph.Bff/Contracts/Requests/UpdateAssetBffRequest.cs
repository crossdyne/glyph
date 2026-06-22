namespace Glyph.Bff.Contracts.Requests
{
    public sealed record UpdateAssetBffRequest(string AssetId, string AssetName, IFormFile File);
}