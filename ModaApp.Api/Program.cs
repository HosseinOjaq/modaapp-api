using ModaApp.Common.Models;
using ModaApp.WebFramework.Swagger;
using Edition.WebFramework.Middlewares;
using ModaApp.WebFramework.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomApiVersioning();
builder.Services.AddControllers();
builder.Services.AddCustomCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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