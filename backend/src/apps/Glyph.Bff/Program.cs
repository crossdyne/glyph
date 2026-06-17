using System.Reflection;
using Glyph.Bff.Extensions;

var builder = WebApplication.CreateBuilder(args);

var executingAssembly = Assembly.GetExecutingAssembly(); 
var configuration = builder.Configuration;
var environment = builder.Environment; 

builder.Services
    // Default
    .AddOpenApi()
    .AddAuthorization()
    // Custom
    .ConfigureOptions()
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