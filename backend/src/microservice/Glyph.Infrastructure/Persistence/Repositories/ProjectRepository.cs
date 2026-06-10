using Glyph.Application.Interfaces.Repositories;
using Glyph.Domain.Models;
using Glyph.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Responses;

namespace Glyph.Infrastructure.Persistence.Repositories
{
    internal sealed class ProjectRepository(GlyphContext context) : Repository<Project, GlyphContext>(context), IProjectRepository
    {
        public async Task<List<ProjectResponse>> GetAllAsync()
            => await _entity.Select(x => new ProjectResponse(x.Id.ToString(), x.Name)).ToListAsync();
    }
}