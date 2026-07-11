using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Glyph.Assets.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Infrastructure.Persistence.Repositories
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

        public async Task<List<AssetMetadataResponse>> GetAggregated(Guid projectId, Guid userId)
            => await _entity
                .AsNoTracking()
                    .Include(a => a.AssetProjects)
                        .Where(a => a.AssetProjects.Any(ap => ap.ProjectId == projectId) && (a.UserId == userId || a.UserId == null))
                            .Select(x => new AssetMetadataResponse(
                                    x.Id.ToString(), 
                                    x.AssetName, 
                                    new S3KeyResponse(x.S3Key.Value, x.S3Key.Bucket, x.S3Key.FileName, x.S3Key.FolderPath), 
                                    x.CategoryId.ToString(),
                                    x.AssetProjects.Select(ap => ap.ProjectId.ToString()).ToList(),
                                    x.UserId == null))
                                        .ToListAsync();

        public async Task<List<AssetMetadataResponse>> GetMetadata(Guid? userId)
        {
            IQueryable<Asset> query = _entity.AsNoTracking().Include(a => a.AssetProjects);  

            if (userId.HasValue)
                query = query.Where(a => a.UserId == userId);
            else
                query = query.Where(a => a.UserId == null);

            return await query.Select(x => 
                new AssetMetadataResponse(
                    x.Id.ToString(), 
                    x.AssetName, 
                    new S3KeyResponse(x.S3Key.Value, x.S3Key.Bucket, x.S3Key.FileName, x.S3Key.FolderPath), 
                    x.CategoryId.ToString(),
                    x.AssetProjects.Select(ap => ap.ProjectId.ToString()).ToList(),
                    x.UserId == null))
                        .ToListAsync();
        }
    }
}