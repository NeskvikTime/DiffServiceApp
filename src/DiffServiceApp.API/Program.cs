using DiffServiceApp.API;
using DiffServiceApp.Application;
using DiffServiceApp.Infrastructure;
using DiffServiceApp.Infrastructure.Persistance.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApiServices()
    .RegisterApplicationServices()
    .RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyMigrations();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
