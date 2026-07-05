using System.Reflection;
using WebApi.Application;
using WebApi.Endpoints;
using WebApi.Extensions;
using WebApi.Extensions.Docs;
using WebApi.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddInfrastructure(builder.Configuration).AddApplication();

WebApplication app = builder.Build();

app.MapApiEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseOpenApiUi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
await app.RunAsync();
