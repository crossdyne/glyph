using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Infrastructure.Persistence.Repositories
{
    internal sealed class CategoryRepository(GlyphContext context) : Repository<Category, GlyphContext>(context), ICategoryRepository
    {
        public async Task<bool> IsGlobal(Guid id) => await _entity.Where(x => x.Id == id).Select(x => x.UserId == null).FirstOrDefaultAsync();

        public async Task<List<CategoryResponse>> GetAllPersonalAsync(Guid userId)
            => await _entity.AsNoTracking().Where(x => x.UserId == userId).Select(x => new CategoryResponse(x.Id.ToString(), x.Name, IsPublic: false)).ToListAsync();

        public async Task<List<CategoryResponse>> GetAllGlobalAsync()
            => await _entity.AsNoTracking().Where(x => x.UserId == null).Select(x => new CategoryResponse(x.Id.ToString(), x.Name, IsPublic: true)).ToListAsync();

        public async Task<List<CategoryResponse>> GetAggregated(Guid userId) 
            => await _entity.AsNoTracking().Where(x => x.UserId == userId || x.UserId == null).Select(x => new CategoryResponse(x.Id.ToString(), x.Name, IsPublic: x.UserId == null)).ToListAsync();
    }
}