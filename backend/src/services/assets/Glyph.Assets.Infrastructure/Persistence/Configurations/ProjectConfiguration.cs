using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glyph.Assets.Infrastructure.Persistence.Configurations
{
    public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("projects");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, db => ProjectId.From(db))
                .ValueGeneratedNever();

            builder.Property(p => p.Name)
                .HasColumnName("name")
                .HasConversion(name => name.Value, db => ProjectName.Create(db))
                .IsRequired();
        }
    }
}