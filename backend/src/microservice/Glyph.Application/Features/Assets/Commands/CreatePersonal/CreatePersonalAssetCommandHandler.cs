using Crossdyne.Toolkit.Results;
using Glyph.Application.Interfaces;
using Glyph.Application.Interfaces.Clients;
using Glyph.Application.Interfaces.Repositories;
using Glyph.Application.Interfaces.Services;
using Glyph.Domain.Models;
using Glyph.Domain.ValueObjects.Assets;
using Glyph.Domain.ValueObjects.Categories;
using Glyph.Domain.ValueObjects.Projects;
using Glyph.Domain.ValueObjects.Shared;
using MediatR;

namespace Glyph.Application.Features.Assets.Commands.CreatePersonal
{
    public sealed class CreatePersonalAssetCommandHandler(
        IAssetRepository repository,
        IUnitOfWork unitOfWork,
        IFileMetadataDetector metadataDetector,
        IFileStorageClient fileStorage) : IRequestHandler<CreatePersonalAssetCommand, Result>
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

                var fileResult = await fileStorage.Upload(s3Key, mimeType, request.FileContent);

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