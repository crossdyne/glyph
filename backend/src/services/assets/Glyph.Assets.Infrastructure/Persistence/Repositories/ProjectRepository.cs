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
            => await _entity.Select(x => new ProjectResponse(x.Id.ToString(), x.Name)).ToListAsync();
    }
}