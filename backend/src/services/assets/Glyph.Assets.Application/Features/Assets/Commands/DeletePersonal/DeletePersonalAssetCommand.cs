using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Assets.Application.Features.Assets.Commands.DeletePersonal
{
    public sealed record DeletePersonalAssetCommand(Guid UserId, Guid AssetId) : IRequest<Result>, IHasUserIdGuid, IHasAssetId;
}