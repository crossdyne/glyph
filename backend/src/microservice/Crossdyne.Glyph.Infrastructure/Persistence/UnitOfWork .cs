using Crossdyne.Glyph.Application.Interfaces;
using Crossdyne.Glyph.Infrastructure.Persistence.Contexts;

namespace Crossdyne.Glyph.Infrastructure.Persistence
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