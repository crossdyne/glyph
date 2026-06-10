using Glyph.Application.Interfaces;
using Glyph.Infrastructure.Persistence.Contexts;

namespace Glyph.Infrastructure.Persistence
{
    internal class UnitOfWork(GlyphContext context) : IUnitOfWork
    {
        private readonly GlyphContext _context = context;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}