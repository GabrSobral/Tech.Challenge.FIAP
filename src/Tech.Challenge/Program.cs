using Tech.Challenge.Application;
using Tech.Challenge.Configuration;
using Tech.Challenge.Infra.Database;
using Tech.Challenge.Infra.MailService;
using Tech.Challenge.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddApplication()
    .AddInfraDatabase(builder.Configuration)
    .AddInfraMailService()
    .AddPresentation()
    .ConfigureCors()
    .ConfigureAuthentication();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ConfigureMiddlewares();
app.ConfigureEndPoints();
app.ConfigureMigrations();

app.Run();
