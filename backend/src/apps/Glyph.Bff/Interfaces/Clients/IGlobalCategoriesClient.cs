using Shared.Contracts.Assets.Responses;
using Shared.Http.Abstraction;

namespace Glyph.Bff.Interfaces.Clients
{
    public interface IGlobalCategoriesClient : IHttpService<CategoryResponse, string>
    {
        
    }
}