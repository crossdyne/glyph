using Glyph.Assets.Infrastructure.Persistence.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WireMock.Server;

namespace Glyph.Assets.Tests.Integration;

public class TestFixture : IDisposable
{
    private readonly SqliteConnection _connection;
    public GlyphContext DbContext { get; }
    public WireMockServer FileServiceMock { get; } = WireMockServer.Start();

    public TestFixture()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<GlyphContext>()
            .UseSqlite(_connection)
            .Options;

        DbContext = new GlyphContext(options);
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        FileServiceMock.Stop();
        DbContext.Dispose();
        _connection.Dispose();
    }
}