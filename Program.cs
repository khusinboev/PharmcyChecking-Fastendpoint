global using FastEndpoints;
using FastEndpoints.Swagger;
using NSwag;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();

builder.Services.AddSwaggerDoc();

builder.Services.AddSwaggerDoc(s =>
{
    s.DocumentName = "Release 1.0";
    s.Title = "Web API";
    s.Version = "v1.0";
    s.AddAuth("ApiKey", new()
    {
        Name = "api_key",
        In = OpenApiSecurityApiKeyLocation.Header,
        Type = OpenApiSecuritySchemeType.ApiKey,
    });
}, addJWTBearerAuth: false);

var app = builder.Build();
app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
});

app.UseSwaggerGen();
app.Run();


