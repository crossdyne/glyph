using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Assets.Application.Features.Assets.Commands.DeleteGlobal
{
    public sealed record DeleteGlobalAssetCommand(Guid AssetId) : IRequest<Result>, IHasAssetId;
}