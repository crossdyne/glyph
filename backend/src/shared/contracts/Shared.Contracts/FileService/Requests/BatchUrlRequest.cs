namespace Shared.Contracts.FileService.Requests
{
    public sealed record BatchUrlRequest(List<FileRequest> Files, int? Expires);
}