using Microsoft.OpenApi.Models;
using API.Infrastructure.IoC;
using Data.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencyRegistrations();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen((c) =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LT Photo Album API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

var appContext = builder.Services.Resolve<IApplicationContext>();
await appContext.Migrate();

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "";
});

app.MapControllers();
app.Run();
