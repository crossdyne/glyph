using Crossdyne.Toolkit.Results;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;
using Shared.Contracts.Assets.Requests;

namespace Glyph.Bff.Features.Categories.Command.CreatePersonal
{
    public sealed class CreatePersonalCategoryCommandHandler(IPersonalCategoriesClient client) : IRequestHandler<CreatePersonalCategoryCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreatePersonalCategoryCommand request, CancellationToken cancellationToken)
            => await client.AddWithResultAsync<string, CreateCategoryRequest>(new CreateCategoryRequest(request.Name)).ToResult();
    }
}