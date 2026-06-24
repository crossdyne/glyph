using Crossdyne.Toolkit.Results;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.DeleteGlobal
{
    public sealed class DeleteGlobalAssetCommandHandler(IGlobalAssetClient client) : IRequestHandler<DeleteGlobalAssetCommand, Result>
    {
        public async Task<Result> Handle(DeleteGlobalAssetCommand request, CancellationToken cancellationToken)
            => await client.DeleteAsync(request.AssetId).ToResult();
    }
}