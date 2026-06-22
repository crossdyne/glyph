namespace Shared.Contracts.FileService.Requests
{
    public sealed record FileRequest(string Bucket, string Folder, string Key);
}