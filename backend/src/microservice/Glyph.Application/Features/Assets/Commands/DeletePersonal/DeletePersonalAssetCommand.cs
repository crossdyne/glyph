using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Assets.Commands.DeletePersonal
{
    public sealed record DeletePersonalAssetCommand(Guid UserId, Guid AssetId) : IRequest<Result>, IHasUserIdGuid, IHasAssetId;
}