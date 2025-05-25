using Edition.WebFramework.Middlewares;
using ModaApp.WebFramework.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomApiVersioning();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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