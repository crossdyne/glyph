using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Application.Interfaces.Services;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Glyph.Assets.Domain.ValueObjects.Shared;
using MediatR;
using Shared.Contracts.FileService.Interfaces;

namespace Glyph.Assets.Application.Features.Assets.Commands.CreatePersonal
{
    public sealed class CreatePersonalAssetCommandHandler(
        IAssetRepository repository,
        IUnitOfWork unitOfWork,
        IFileMetadataDetector metadataDetector,
        IFileServiceClient fileStorage) : IRequestHandler<CreatePersonalAssetCommand, Result>
    {
        public async Task<Result> Handle(CreatePersonalAssetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var detected = await metadataDetector.DetectAsync(request.FileContent, request.FileName, cancellationToken);

                if (request.FileContent.CanSeek)
                    request.FileContent.Position = 0;

                var s3Key = S3Key.Create(request.Bucket, [.. request.Folders], request.FileName);

                var format = Format.FromName(detected.FormatName);
                var mimeType = MimeType.FromFormat(format); 
                var assetType= AssetType.FromName(detected.AssetTypeName);
                var sizeBytes = SizeBytes.Create(request.SizeBytes);

                var categoryId = CategoryId.From(request.CategoryId);
                var projectIds = request.ProjectIds.Select(x => ProjectId.From(Guid.Parse(x))).ToList();
                var userId = UserId.From(request.UserId);

                Asset asset = Asset.Create(s3Key, assetType, format, mimeType, sizeBytes, categoryId, projectIds, userId);

                var fileResult = await fileStorage.Upload(s3Key.Bucket, s3Key.FolderPath, s3Key.FileName, mimeType.Value, request.FileContent);

                if (fileResult.IsFailure)
                    return fileResult;

                await repository.AddAsync(asset, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание ассета: {ex}"));
            }
        }
    }
}