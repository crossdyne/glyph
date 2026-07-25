using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.ValueObjects.Shared;
using Microsoft.Extensions.Logging;
using Shared.Contracts.FileService.Interfaces;
using Shared.Contracts.Messaging.Events;
using Shared.Contracts.Messaging.Interfaces;

namespace Glyph.Assets.Application.Features.Account.EventHandlers
{
    public sealed class UserAccountDeletedIntegrationEventHandler(
        IAssetRepository assetRepository,
        ICategoryRepository categoryRepository,
        IFileServiceClient fileServiceClient,
        IUnitOfWork unitOfWork,
        ILogger<UserAccountDeletedIntegrationEventHandler> logger) : IIntegrationEventHandler<UserAccountDeletedIntegrationEvent>
    {
        public async Task HandleAsync(UserAccountDeletedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            logger.LogInformation("Получение событие удаление учетной записи UserId: {UserId}", @event.UserId);

            var userId = UserId.From(@event.UserId);

            var assets = await assetRepository.GetMetadata(@event.UserId);

            foreach (var asset in assets)
            {
                try
                {
                    await fileServiceClient.Delete(asset.S3Key.Bucket, asset.S3Key.FolderPath, asset.S3Key.Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Неу далось удалить асет {AssetKey} во время удаление аккаунта юзера {UserId}", asset.S3Key.Name, @event.UserId.ToString());
                    throw;
                }
            }

            int removedAssets = await assetRepository.RemoveAllAsync(userId);
            int removedCategories = await categoryRepository.RemoveAllAsync(userId);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Обработка событие удаление учетной записи для UserId: {UserId} выполнена успешно. Было удалено {removedAssets} ассетов, {removedCategories} категорий", @event.UserId, removedAssets, removedCategories);
        }
    }
}