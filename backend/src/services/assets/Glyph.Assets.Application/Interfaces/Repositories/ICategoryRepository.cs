using Glyph.Assets.Domain.Models;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> IsGlobal(Guid id);
        Task<List<CategoryResponse>> GetAllAsync(Guid userId);
        Task<List<CategoryResponse>> GetAllGlobalAsync();
        Task<List<CategoryResponse>> GetPersonalAndGlobal(Guid userId);
    }
}