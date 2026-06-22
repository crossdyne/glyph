using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Application.Interfaces.Services;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using MediatR;
using Shared.Contracts.FileService.Interfaces;

namespace Glyph.Assets.Application.Features.Assets.Commands.UpdatePersonal
{
    public sealed class UpdatePersonalAssetCommandHandler(
        IAssetRepository repository,
        IUnitOfWork unitOfWork,
        IFileMetadataDetector detector,
        IFileServiceClient storageClient) : IRequestHandler<UpdatePersonalAssetCommand, Result>
    {
        public async Task<Result> Handle(UpdatePersonalAssetCommand request, CancellationToken cancellationToken)
        {
            string storageBucket = null!;
            string storageFolderPath = null!;
            string storageKey = null!;

            try
            {
                Maybe<Asset> maybe = await repository.GetByAsync(x => x.Id == request.AssetId && x.UserId == request.UserId, cancellationToken);

                if (maybe.IsNone)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Ассет не был найден"));

                Asset storageAsset = maybe.Value;

                storageBucket = storageAsset.S3Key.Bucket;
                storageFolderPath = storageAsset.S3Key.FolderPath;
                storageKey = storageAsset.S3Key.FileName;

                var detected = await detector.DetectAsync(request.FileContent, request.FileName, cancellationToken);
                
                var s3Key = S3Key.Create(storageAsset.S3Key.Bucket, [.. storageAsset.S3Key.Folders], request.FileName);
                var format = Format.FromName(detected.FormatName);
                var mimeType = MimeType.FromFormat(format); 
                var assetType= AssetType.FromName(detected.AssetTypeName);
                var sizeBytes = SizeBytes.Create(request.SizeBytes);

                if (request.FileContent.CanSeek)
                    request.FileContent.Position = 0;

                var uploadResult = await storageClient.Upload(s3Key.Bucket, s3Key.FolderPath, s3Key.FileName, mimeType.Value, request.FileContent);

                if (uploadResult.IsFailure)
                    return uploadResult;  

                storageAsset.UpdateContent(s3Key, format, mimeType, assetType, sizeBytes);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                await storageClient.Delete(storageBucket, storageFolderPath, storageKey);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Update, $"Произошла критическая ошибка при обновление метаданных :{ex}"));
            }
        }
    }
}