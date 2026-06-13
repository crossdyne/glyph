using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glyph.Assets.Infrastructure.Persistence.Configurations
{
    public sealed class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("assets");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, db => AssetId.From(db))
                .ValueGeneratedNever();
            
            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .HasConversion(userId => userId.HasValue ? userId.Value.Value : (Guid?)null, db => db == null ? null : UserId.From(db.Value))
                .IsRequired(false);

            builder.Property(a => a.CategoryId)
                .HasColumnName("category_id")
                .HasConversion(id => id.Value, db => CategoryId.From(db))
                .IsRequired();

            builder.Property(a => a.S3Key)
                .HasColumnName("s3_key")
                .HasConversion(key => key.Value, db => S3Key.Restore(db))
                .IsRequired();

            builder.Property(a => a.AssetType)
                .HasColumnName("asset_type")
                .HasConversion(assetType => assetType.Value, db => AssetType.FromValue(db))
                .IsRequired();

            builder.Property(a => a.Format)
                .HasColumnName("format")
                .HasConversion(f => f.Value, db => Format.FromValue(db))
                .IsRequired();

            builder.Property(a => a.MimeType)
                .HasColumnName("mime_type")
                .HasConversion(mimeType => mimeType.Value, db => MimeType.Create(db))
                .IsRequired();
                
            builder.Property(a => a.SizeBytes)
                .HasColumnName("size_bytes")
                .HasConversion(sizeBytes => sizeBytes.Value, db => SizeBytes.Create(db))
                .IsRequired();
                                
            builder.Property(a => a.IsPublic)
                .HasColumnName("is_public")
                .IsRequired();
                                
            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
                               
            builder.Property(a => a.UpdateAt)
                .HasColumnName("update_at")
                .IsRequired();

            builder.HasMany(a => a.AssetProjects)
                .WithOne()
                .HasForeignKey(ap => ap.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.AssetProjects).HasField("_assetProjects").UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}