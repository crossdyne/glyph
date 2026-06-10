using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Errors;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Clients;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Application.Interfaces.Services;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using MediatR;

namespace Glyph.Assets.Application.Features.Assets.Commands.CreateGlobal
{
    public sealed class CreateGlobalAssetCommandHandler(
        IAssetRepository assetRepository, 
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IFileMetadataDetector metadataDetector,
        IFileStorageClient fileStorage) : IRequestHandler<CreateGlobalAssetCommand, Result>
    {
        public async Task<Result> Handle(CreateGlobalAssetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!await categoryRepository.IsGlobal(request.CategoryId))
                    return Result.Failure(new Error(AppErrors.CategoryIsPersonal, "Выбранная вами категория не является общей, пожалуйста выберите другую."));

                var detected = await metadataDetector.DetectAsync(request.FileContent, request.FileName, cancellationToken);

                if (request.FileContent.CanSeek)
                    request.FileContent.Position = 0;

                var s3Key = S3Key.Create(request.Bucket, [.. request.Folders], request.FileName);

                var format = Format.FromName(detected.FormatName);
                var mimeType = MimeType.FromFormat(format); 
                var assetType= AssetType.FromName(detected.AssetTypeName);
                var sizeBytes = SizeBytes.Create(request.SizeBytes);
                var categoryId = CategoryId.From(request.CategoryId);

                Asset asset = Asset.Create(s3Key, assetType, format, mimeType, sizeBytes, categoryId);

                await assetRepository.AddAsync(asset, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                var fileResult = await fileStorage.Upload(s3Key, mimeType, request.FileContent);

                if (fileResult.IsFailure)
                    return fileResult;

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание ассета: {ex}"));
            }
        }
    }
}