namespace Shared.Contracts.Requests
{
    public sealed record CreateAssetRequest(
        string Bucket, 
        string FoldersJson, 
        string FileName,
        string CategoryId,
        string ProjectIdsJson,
        string AssetName);
}