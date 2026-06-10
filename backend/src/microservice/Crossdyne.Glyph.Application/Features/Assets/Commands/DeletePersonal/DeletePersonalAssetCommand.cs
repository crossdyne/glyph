using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Assets.Commands.DeletePersonal
{
    public sealed record DeletePersonalAssetCommand(Guid UserId, Guid AssetId) : IRequest<Result>, IHasUserIdGuid, IHasAssetId;
}