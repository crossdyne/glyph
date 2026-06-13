using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glyph.Assets.Infrastructure.Persistence.Configurations
{
    public sealed class AssetProjectsConfiguration : IEntityTypeConfiguration<AssetProjects>
    {
        public void Configure(EntityTypeBuilder<AssetProjects> builder)
        {
            builder.ToTable("asset_projects");
            builder.HasKey(ap => new { ap.AssetId, ap.ProjectId });

            builder.Property(ap => ap.AssetId)
                .HasColumnName("asset_id")
                .HasConversion(assetId => assetId.Value, db => AssetId.From(db))
                .ValueGeneratedNever();

            builder.Property(ap => ap.ProjectId)
                .HasColumnName("project_id")
                .HasConversion(assetId => assetId.Value, db => ProjectId.From(db))
                .ValueGeneratedNever();

            builder.HasOne<Asset>()
                .WithMany()
                .HasForeignKey(ap => ap.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Project>()
                .WithMany()
                .HasForeignKey(ap => ap.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}