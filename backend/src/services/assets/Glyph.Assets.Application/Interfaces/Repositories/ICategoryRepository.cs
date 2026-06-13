using Glyph.Assets.Domain.Models;
using Shared.Contracts.Responses;

namespace Glyph.Assets.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> IsGlobal(Guid id);
        Task<List<CategoryResponse>> GetAllAsync(Guid userId);
        Task<List<CategoryResponse>> GetAllGlobalAsync();
    }
}