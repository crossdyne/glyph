using Crossdyne.Toolkit.Results;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;

namespace Glyph.Bff.Features.Categories.Command.DeleteGlobal
{
    public sealed class DeleteGlobalCategoryCommandHandler(IGlobalCategoriesClient client) : IRequestHandler<DeleteGlobalCategoryCommand, Result>
    {
        public async Task<Result> Handle(DeleteGlobalCategoryCommand request, CancellationToken cancellationToken)
            => await client.DeleteAsync(request.AssetId).ToResult();
    }
}