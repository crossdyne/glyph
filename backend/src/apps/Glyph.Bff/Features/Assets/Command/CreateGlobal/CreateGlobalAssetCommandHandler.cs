using Crossdyne.Toolkit.Results;
using Glyph.Bff.Constants;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.CreateGlobal
{
    public sealed class CreateGlobalAssetCommandHandler(IGlobalAssetClient client) : IRequestHandler<CreateGlobalAssetCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateGlobalAssetCommand request, CancellationToken cancellationToken)
            => await client.Create(
                FileStorageConstants.Bucket, 
                FileStorageConstants.GlobalAssetsSvgFolders, 
                request.FileName, 
                request.CategoryId, 
                request.ProjectIdsJson, 
                request.File,
                request.AssetName).CatchAsync();
    }
}