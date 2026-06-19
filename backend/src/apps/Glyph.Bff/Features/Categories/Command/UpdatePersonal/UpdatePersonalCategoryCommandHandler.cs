using Crossdyne.Toolkit.Results;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;
using Shared.Contracts.Requests;

namespace Glyph.Bff.Features.Categories.Command.UpdatePersonal
{
    public sealed class UpdatePersonalCategoryCommandHandler(IPersonalCategoriesClient client) : IRequestHandler<UpdatePersonalCategoryCommand, Result>
    {
        public async Task<Result> Handle(UpdatePersonalCategoryCommand request, CancellationToken cancellationToken)
            => await client.UpdateAsync(request.CategoryId, new UpdateCategoryRequest(request.Name)).ToResult();
    }
}