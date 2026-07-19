using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Application.Interfaces.Services;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using MediatR;
using Shared.Contracts.FileService.Interfaces;

namespace Glyph.Assets.Application.Features.Assets.Commands.UpdateGlobal
{
    public class UpdateGlobalAssetCommandHandler(
        IAssetRepository repository, 
        IUnitOfWork unitOfWork, 
        IFileMetadataDetector detector, 
        IFileServiceClient storageClient) : IRequestHandler<UpdateGlobalAssetCommand, Result>
    {
        public async Task<Result> Handle(UpdateGlobalAssetCommand request, CancellationToken cancellationToken)
        {
            Maybe<Asset> maybe = await repository.GetByAsync(x => x.Id == request.AssetId, cancellationToken);

            if (maybe.IsNone)
                return Result.Failure(new Error(ErrorCode.NotFound, "Ассет не был найден"));

            Asset storageAsset = maybe.Value;

            string oldBucket = storageAsset.S3Key.Bucket;
            string oldFolderPath = storageAsset.S3Key.FolderPath;
            string oldFileName = storageAsset.S3Key.FileName;

            var categoryId = CategoryId.From(Guid.Parse(request.CategoryId));
            var assetName = AssetName.Create(request.AssetName);

            var fileCondition = request.FileContent != null 
                             && request.FileContent.Length > 0 
                             && !string.IsNullOrWhiteSpace(request.FileName) 
                             && request.SizeBytes != null;

            if (fileCondition)
            {
                var detected = await detector.DetectAsync(request.FileContent!, request.FileName!, cancellationToken);
            
                var s3Key = S3Key.Create(storageAsset.S3Key.Bucket, [.. storageAsset.S3Key.Folders], request.FileName!);
                var format = Format.FromName(detected.FormatName);
                var mimeType = MimeType.FromFormat(format); 
                var assetType= AssetType.FromName(detected.AssetTypeName);
                var sizeBytes = SizeBytes.Create(request.SizeBytes!.Value);

                if (request.FileContent!.CanSeek)
                    request.FileContent.Position = 0;
  
                var uploadResult = await storageClient.Upload(s3Key.Bucket, s3Key.FolderPath, s3Key.FileName, mimeType.Value, request.FileContent);

                if (uploadResult.IsFailure)
                    return uploadResult; 

                storageAsset.UpdateContent(s3Key, format, mimeType, assetType, sizeBytes);
            }

            storageAsset.UpdateName(assetName);
            storageAsset.AttachCategory(categoryId);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (fileCondition)
            {
                try
                {
                    await storageClient.Delete(oldBucket, oldFolderPath, oldFileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось удалить старый файл {oldFileName} из S3: {ex.Message}");
                }
            }
  
            return Result.Success();
        }
    }
}