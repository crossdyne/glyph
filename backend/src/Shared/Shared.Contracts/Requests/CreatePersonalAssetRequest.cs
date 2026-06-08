namespace Shared.Contracts.Requests
{
    public sealed record CreatePersonalAssetRequest(
        string Bucket, 
        string FoldersJson, 
        string FileName,
        string CategoryId,
        string ProjectIdsJson,
        string UserId);
}