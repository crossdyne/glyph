using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Application.Interfaces;
using Glyph.Application.Interfaces.Clients;
using Glyph.Application.Interfaces.Repositories;
using Glyph.Application.Interfaces.Services;
using Glyph.Domain.Models;
using Glyph.Domain.ValueObjects.Assets;
using MediatR;

namespace Glyph.Application.Features.Assets.Commands.UpdateGlobal
{
    public class UpdateGlobalAssetCommandHandler(
        IAssetRepository repository, 
        IUnitOfWork unitOfWork, 
        IFileMetadataDetector detector, 
        IFileStorageClient storageClient) : IRequestHandler<UpdateGlobalAssetCommand, Result>
    {
        public async Task<Result> Handle(UpdateGlobalAssetCommand request, CancellationToken cancellationToken)
        {
            string storageBucket = null!;
            string storageFolderPath = null!;
            string storageKey = null!;

            try
            {
                Maybe<Asset> maybe = await repository.GetByAsync(x => x.Id == request.AssetId, cancellationToken);

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

                storageAsset.UpdateContent(s3Key, format, mimeType, assetType, sizeBytes);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                await storageClient.Delete(storageBucket, storageFolderPath, storageKey);

                var uploadResult = await storageClient.Upload(s3Key, mimeType, request.FileContent);

                if (uploadResult.IsFailure)
                    return uploadResult;  
                    
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Update, $"Произошла критическая ошибка при обновление метаданных :{ex}"));
            }
        }
    }
}