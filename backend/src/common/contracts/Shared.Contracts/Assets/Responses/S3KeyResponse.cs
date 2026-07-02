namespace Shared.Contracts.Assets.Responses
{
    public sealed record S3KeyResponse(string Key, string Bucket, string Name, string FolderPath);
}