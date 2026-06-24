using Crossdyne.Toolkit.Results;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;
using Shared.Contracts.Assets.Requests;

namespace Glyph.Bff.Features.Categories.Command.UpdateGlobal
{
    public sealed class UpdateGlobalCategoryCommandHandler(IGlobalCategoriesClient client) : IRequestHandler<UpdateGlobalCategoryCommand, Result>
    {
        public async Task<Result> Handle(UpdateGlobalCategoryCommand request, CancellationToken cancellationToken)
            => await client.UpdateAsync(request.AssetId, new UpdateCategoryRequest(request.Name)).ToResult();
    }
}