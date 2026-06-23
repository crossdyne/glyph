namespace Glyph.Bff.Contracts.Requests
{
    public sealed record CreateAssetBffRequest(
        string CategoryId,
        string ProjectIdsJson,
        IFormFile File,
        string AssetName);
}