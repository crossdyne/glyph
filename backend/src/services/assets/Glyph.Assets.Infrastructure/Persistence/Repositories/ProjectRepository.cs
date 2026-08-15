using Crossdyne.Toolkit.Primitives;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Infrastructure.Persistence.Repositories
{
    internal sealed class ProjectRepository(GlyphContext context) : Repository<Project, GlyphContext>(context), IProjectRepository
    {
        public async Task<List<ProjectResponse>> GetAllAsync()
            => await _entity.AsNoTracking().Select(x => new ProjectResponse(x.Id.ToString(), x.Name)).ToListAsync();

        public async Task<Maybe<Project>> GetProjectByCode(string projectCode)
            => Maybe<Project>.Some(await _entity.AsNoTracking().FirstOrDefaultAsync(p => p.Code == projectCode));
    }
}