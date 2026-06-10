using Glyph.Application.Interfaces.Repositories;
using Glyph.Domain.Models;
using Glyph.Domain.ValueObjects.Projects;
using Glyph.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Responses;

namespace Glyph.Infrastructure.Persistence.Repositories
{
    internal sealed class AssetRepository(GlyphContext context) : Repository<Asset, GlyphContext>(context), IAssetRepository
    {
        public override void Remove(Asset entity)
        {
            var assetProjects = _context.Set<AssetProjects>();
            var links = assetProjects.Where(ap => ap.AssetId == entity.Id).ToList();
            assetProjects.RemoveRange(links);

            base.Remove(entity);
        }

        public async Task<bool> HasProjectsLinksAsync(ProjectId projectId, CancellationToken cl)
            => await _context.Set<AssetProjects>().AnyAsync(ap => ap.ProjectId == projectId, cl);

        public async Task<List<AssetResponse>> GetAllByFiler(Guid projectId, Guid? userId)
        {
            IQueryable<Asset> query = _entity.AsNoTracking().Where(a => a.AssetProjects.Any(ap => ap.ProjectId == projectId));

            if (userId.HasValue)
                query = query.Where(a => a.UserId == userId);

            return await query.Select(x => new AssetResponse(x.Id.ToString(), x.S3Key.FileName, x.S3Key.Value)).ToListAsync();
        }
    }
}