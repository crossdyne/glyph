using Crossdyne.Toolkit.Results;
using Glyph.Bff.Constants;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.CreatePersonal
{
    public sealed class CreatePersonalAssetCommandHandler(IPersonalAssetClient client) : IRequestHandler<CreatePersonalAssetCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreatePersonalAssetCommand request, CancellationToken cancellationToken)
            => await client.Create(
                FileStorageConstants.Bucket, 
                FileStorageConstants.PersonalFolders, 
                request.FileName, 
                request.CategoryId, 
                request.ProjectIdsJson, 
                request.File).CatchAsync();
    }
}