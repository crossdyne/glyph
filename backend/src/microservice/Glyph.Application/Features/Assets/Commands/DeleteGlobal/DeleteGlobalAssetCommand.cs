using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Assets.Commands.DeleteGlobal
{
    public sealed record DeleteGlobalAssetCommand(Guid AssetId) : IRequest<Result>, IHasAssetId;
}