using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Shared;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> IsGlobal(Guid id);
        Task<List<CategoryResponse>> GetAllPersonalAsync(Guid userId);
        Task<List<CategoryResponse>> GetAllGlobalAsync();
        Task<List<CategoryResponse>> GetAggregated(Guid userId);
        Task<int> RemoveAllAsync(UserId userId);
    }
}