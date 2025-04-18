using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BlazorApplicationInsights;
using BWHazel.Portfolio.Web;
using BWHazel.Portfolio.Web.Models;
using BWHazel.Portfolio.Web.Models.Validators;
using BWHazel.Portfolio.Web.Services;
using FluentValidation;
using MudBlazor.Services;

const string SiteContentFile = "content.json";
const string SiteTitleKey = "Site:Title";
const string SiteVersionKey = "Site:Version";

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

HttpClient httpClient = new()
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};

builder.Services.AddScoped(serviceProvider => httpClient);
builder.Services.AddScoped<IValidator<Album>, AlbumValidator>();
builder.Services.AddScoped<IValidator<MuseScoreUser>, MuseScoreUserValidator>();
builder.Services.AddScoped<IValidator<Score>, ScoreValidator>();
builder.Services.AddScoped<IValidator<Site>, SiteValidator>();
builder.Services.AddScoped<AlbumService>();
builder.Services.AddScoped<ScoreService>();
builder.Services.AddScoped<BehanceProjectService>();
builder.Services.AddScoped<SiteListService>();
builder.Services.AddScoped<ITelemetryService, ApplicationInsightsTelemetryService>();

builder.Configuration
    .AddJsonStream(await GetContentData(httpClient));

builder.Services.AddCascadingValue("SiteTitle", provider => builder.Configuration[SiteTitleKey]);
builder.Services.AddCascadingValue("SiteVersion", provider => builder.Configuration[SiteVersionKey]);

builder.Services.AddMudServices();

builder.Services.AddBlazorApplicationInsights(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("AzureApplicationInsights");
});

await builder
    .Build()
    .RunAsync();

/// <summary>
/// Gets the site content data.
/// </summary>
/// <param name="httpClient">The HTTP client.</param>
/// <returns>The site content data.</returns>
static async Task<Stream> GetContentData(HttpClient httpClient)
{
    HttpResponseMessage contentConfigurationResponse = await httpClient.GetAsync(SiteContentFile);
    Stream? contentConfigurationStream = await contentConfigurationResponse.Content.ReadAsStreamAsync();
    return contentConfigurationStream;
}