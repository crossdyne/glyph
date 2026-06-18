using Crossdyne.Toolkit.Results;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using MediatR;

namespace Glyph.Bff.Features.Categories.Command.DeletePersonal
{
    public sealed class DeletePersonalCategoryCommandHandler(IPersonalCategoriesClient client) : IRequestHandler<DeletePersonalCategoryCommand, Result>
    {
        public async Task<Result> Handle(DeletePersonalCategoryCommand request, CancellationToken cancellationToken)
            => await client.DeleteAsync(request.CategoryId).ToResult();
    }
}