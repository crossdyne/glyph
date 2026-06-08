using Crossdyne.Glyph.Application.Extensions;
using Crossdyne.Glyph.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.UseHttpsRedirection();
app.Run();