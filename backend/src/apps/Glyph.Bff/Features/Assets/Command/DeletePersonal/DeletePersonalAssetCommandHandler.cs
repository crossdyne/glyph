using Crossdyne.Toolkit.Results;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.DeletePersonal
{
    public sealed class DeletePersonalAssetCommandHandler(IPersonalAssetClient client) : IRequestHandler<DeletePersonalAssetCommand, Result>
    {
        public async Task<Result> Handle(DeletePersonalAssetCommand request, CancellationToken cancellationToken)
            => await client.DeleteAsync(request.AssetId).ToResult();
    }
}