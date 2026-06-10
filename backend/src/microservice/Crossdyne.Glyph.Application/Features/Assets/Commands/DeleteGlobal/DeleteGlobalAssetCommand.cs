using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Assets.Commands.DeleteGlobal
{
    public sealed record DeleteGlobalAssetCommand(Guid AssetId) : IRequest<Result>, IHasAssetId;
}