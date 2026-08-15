using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glyph.Assets.Infrastructure.Persistence.Configurations
{
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, db => CategoryId.From(db))
                .ValueGeneratedNever();

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .HasConversion(userId => userId.HasValue ? userId.Value.Value : (Guid?)null, db => db == null ? null : UserId.From(db.Value))
                .IsRequired(false);

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .HasConversion(name => name.Value, db => CategoryName.Create(db))
                .IsRequired();

            builder.Property(c => c.IsPublic)
                .HasColumnName("is_public")
                .IsRequired();
        }
    }
}