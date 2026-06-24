using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Errors;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Application.Interfaces.Services;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Projects;
using MediatR;
using Shared.Contracts.FileService.Interfaces;

namespace Glyph.Assets.Application.Features.Assets.Commands.CreateGlobal
{
    public sealed class CreateGlobalAssetCommandHandler(
        IAssetRepository assetRepository, 
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IFileMetadataDetector metadataDetector,
        IFileServiceClient fileStorage) : IRequestHandler<CreateGlobalAssetCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateGlobalAssetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!await categoryRepository.IsGlobal(request.CategoryId))
                    return Result<string>.Failure(new Error(AppErrors.CategoryIsPersonal, "Выбранная вами категория не является общей, пожалуйста выберите другую."));

                var detected = await metadataDetector.DetectAsync(request.FileContent, request.FileName, cancellationToken);

                if (request.FileContent.CanSeek)
                    request.FileContent.Position = 0;

                var assetName = AssetName.Create(request.AssetName);
                var s3Key = S3Key.Create(request.Bucket, [.. request.Folders], request.FileName);

                var format = Format.FromName(detected.FormatName);
                var mimeType = MimeType.FromFormat(format); 
                var assetType= AssetType.FromName(detected.AssetTypeName);
                var sizeBytes = SizeBytes.Create(request.SizeBytes);

                var categoryId = CategoryId.From(request.CategoryId);
                var projectIds = request.ProjectIds.Select(x => ProjectId.From(Guid.Parse(x))).ToList();

                Asset asset = Asset.Create(assetName, s3Key, assetType, format, mimeType, sizeBytes, categoryId, projectIds, userId: null);

                await assetRepository.AddAsync(asset, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                var fileResult = await fileStorage.Upload(s3Key.Bucket, s3Key.FolderPath, s3Key.FileName, mimeType.Value, request.FileContent);

                if (fileResult.IsFailure)
                    return Result<string>.Failure(fileResult.Errors);

                return Result<string>.Success(asset.Id.ToString());
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание ассета: {ex}"));
            }
        }
    }
}