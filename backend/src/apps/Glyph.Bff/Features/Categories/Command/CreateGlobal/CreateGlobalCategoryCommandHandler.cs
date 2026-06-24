using Crossdyne.Toolkit.Results;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;
using Shared.Contracts.Assets.Requests;

namespace Glyph.Bff.Features.Categories.Command.CreateGlobal
{
    public sealed class CreateGlobalCategoryCommandHandler(IGlobalCategoriesClient client) : IRequestHandler<CreateGlobalCategoryCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateGlobalCategoryCommand request, CancellationToken cancellationToken)
            => await client.AddWithResultAsync<string, CreateCategoryRequest>(new CreateCategoryRequest(request.Name)).ToResult();
    }
}