using ModaApp.Application;
using ModaApp.Common.Models;
using ModaApp.Infrastructure;
using ModaApp.Api.Extensions;
using ModaApp.WebFramework.Swagger;
using ModaApp.WebFramework.Middlewares;
using ModaApp.WebFramework.Configuration;
using ModaApp.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.AddAutofactServiceProviderAndInterceptors();

builder.Services.AddCustomApiVersioning();
builder.Services.AddControllers();
builder.Services.AddCustomCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterApplicationServices()
                .RegisterInfrastructureServices(builder.Configuration)
                .RegisterPersistenceServices(builder.Configuration);
var siteSettingsConfiguration = builder.Configuration.GetSection(nameof(SiteSettings));
var siteSettings = siteSettingsConfiguration.Get<SiteSettings>();

builder.Services.AddElmahCore(builder.Configuration, siteSettings!.ElmahPath);
builder.Services.AddSwagger();
builder.Services.Configure<SiteSettings>(siteSettingsConfiguration);
builder.Services.AddCustomApiVersioning();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UserCustomeForwardedHeaders();
app.UseCustomExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("CustomCors");
app.UseAuthorization();
app.MapControllers();
app.Run();