using System.Reflection;
using Glyph.Bff.Extensions;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

var executingAssembly = Assembly.GetExecutingAssembly(); 
var configuration = builder.Configuration;
var environment = builder.Environment; 

builder.Logging.ClearProviders();
builder.Host.AddSerilogLogger(); 

builder.Services
    // Default
    .AddOpenApi()
    .AddAuthorization()
    .AddHttpContextAccessor()
    // Custom
    .ConfigureOptions()
    .AddDelegationsHandlers()
    .AddServices(configuration)
    .AddHttpClients(configuration)
    .AddDistributedLock()
    .UseCors()
    .AddSharedCryptoKeyForDecryptCookie(configuration)
    .AddCookie(environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowLocalFrontend");
app.UseAuthentication(); 
app.UseAuthorization();  
app.MapEndpoints(executingAssembly);
app.Run();