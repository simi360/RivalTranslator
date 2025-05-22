using RivalTranslator.Server.Configuration;
using RivalTranslator.Server.Services;
using System.Net;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

builder.Services.Configure<AzureTranslatorOptions>(
    builder.Configuration.GetSection("AzureTranslator"));

builder.Services
    .AddHttpClient<ITranslator, AzureCognitiveTranslator>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseProxy = true,
        DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials,
        SslProtocols = SslProtocols.Tls12
    });

builder.Services.AddControllers();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
