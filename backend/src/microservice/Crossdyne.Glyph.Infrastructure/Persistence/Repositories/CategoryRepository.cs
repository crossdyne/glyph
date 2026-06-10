using Crossdyne.Glyph.Application.Interfaces.Repositories;
using Crossdyne.Glyph.Domain.Models;
using Crossdyne.Glyph.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Responses;

namespace Crossdyne.Glyph.Infrastructure.Persistence.Repositories
{
    internal sealed class CategoryRepository(GlyphContext context) : Repository<Category, GlyphContext>(context), ICategoryRepository
    {
        public async Task<bool> IsGlobal(Guid id) => await _entity.Where(x => x.Id == id).Select(x => x.UserId == null).FirstOrDefaultAsync();

        public async Task<List<CategoryResponse>> GetAllAsync(Guid userId)
            => await _entity.AsNoTracking().Where(x => x.UserId == userId).Select(x => new CategoryResponse(x.Id.ToString(), x.Name, x.UserId.HasValue ? x.UserId.Value.ToString() : null)).ToListAsync();

        public async Task<List<CategoryResponse>> GetAllGlobalAsync()
            => await _entity.AsNoTracking().Select(x => new CategoryResponse(x.Id.ToString(), x.Name, null)).ToListAsync();
    }
}