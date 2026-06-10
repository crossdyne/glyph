using Crossdyne.Glyph.Application.Interfaces.Repositories;
using Crossdyne.Glyph.Domain.Models;
using Crossdyne.Glyph.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Responses;

namespace Crossdyne.Glyph.Infrastructure.Persistence.Repositories
{
    internal sealed class ProjectRepository(GlyphContext context) : Repository<Project, GlyphContext>(context), IProjectRepository
    {
        public async Task<List<ProjectResponse>> GetAllAsync()
            => await _entity.Select(x => new ProjectResponse(x.Id.ToString(), x.Name)).ToListAsync();
    }
}