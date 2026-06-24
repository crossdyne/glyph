using Crossdyne.Toolkit.Results;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.UpdateGlobal
{
    public sealed class UpdateGlobalAssetCommandHandler(IGlobalAssetClient client) : IRequestHandler<UpdateGlobalAssetCommand, Result>
    {
        public async Task<Result> Handle(UpdateGlobalAssetCommand request, CancellationToken cancellationToken)
            => await client.UpdateAsync(request.AssetId, request.AssetName, request.File, request.FileName, request.CategoryId).ToResult();
    }
}