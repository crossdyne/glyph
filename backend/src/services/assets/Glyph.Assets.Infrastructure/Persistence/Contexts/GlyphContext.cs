using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Glyph.Assets.Infrastructure.Persistence.Contexts
{
    public sealed class GlyphContext(DbContextOptions<GlyphContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}