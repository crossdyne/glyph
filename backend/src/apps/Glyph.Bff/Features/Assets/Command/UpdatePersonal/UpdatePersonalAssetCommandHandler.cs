using Crossdyne.Toolkit.Results;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.UpdatePersonal
{
    public sealed class UpdatePersonalAssetCommandHandler(IPersonalAssetClient client) : IRequestHandler<UpdatePersonalAssetCommand, Result>
    {
        public async Task<Result> Handle(UpdatePersonalAssetCommand request, CancellationToken cancellationToken)
            => await client.UpdateAsync(request.AssetId, request.AssetName, request.File, request.FileName, request.CategoryId).ToResult();
    }
}