using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Clients;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using MediatR;

namespace Glyph.Assets.Application.Features.Assets.Commands.DeleteGlobal
{
    public sealed class DeleteGlobalAssetCommandHandler(
        IAssetRepository repository,
        IUnitOfWork unitOfWork,
        IFileStorageClient fileStorage) : IRequestHandler<DeleteGlobalAssetCommand, Result>
    {
        public async Task<Result> Handle(DeleteGlobalAssetCommand request, CancellationToken cancellationToken)
        {            
            try
            {
                Maybe<Asset> maybe = await repository.GetByAsync(x => x.Id == request.AssetId);

                if (maybe.IsNone)
                    return Result.Failure(new Error(ErrorCode.Delete, "Ассет не найден"));

                Asset asset = maybe.Value;

                repository.Remove(asset);
                
                await unitOfWork.SaveChangesAsync(cancellationToken);

                var fileResult = await fileStorage.Delete(asset.S3Key.Bucket, asset.S3Key.FolderPath, asset.S3Key.FileName);

                if (fileResult.IsFailure)
                    return fileResult;

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Произошла критическая ошибка при удаление: {ex}"));
            }
        }
    }
}