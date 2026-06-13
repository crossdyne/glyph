using Shared.Contracts.Responses;

namespace Glyph.Bff.Infrastructure.Clients
{
    public interface IPersonalCategoriesClient
    {
        Task<List<CategoryResponse>> GetAll(string userId);
    }
}