namespace Shared.Contracts.Assets.Requests
{
    public sealed record CreateGlobalAssetRequest(
        string Bucket, 
        string FoldersJson, 
        string FileName,
        string CategoryId,
        string ProjectIdsJson,
        string AssetName);
}